using System;
using System.Printing;
using System.Windows;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace Utils.FlowDocumentUtils
{
    [UsedImplicitly]
    public static class FlowDocumentUtils
    {
        [UsedImplicitly]
        public static void Print(this FlowDocument document)
        {
            // Clone the source document's content into a new FlowDocument. This is because the
            // pagination for the printer needs to be done differently than the pagination for the
            // displayed page. We print the copy, rather that the original FlowDocument.
            var s = new System.IO.MemoryStream();
            var source = new TextRange(document.ContentStart, document.ContentEnd);
            source.Save(s, DataFormats.Xaml);
            var copy = new FlowDocument();
            var dest = new TextRange(copy.ContentStart, copy.ContentEnd);
            dest.Load(s, DataFormats.Xaml);

            // Create a XpsDocumentWriter object, implicitly opening a Windows common print dialog,
            // and allowing the user to select a printer.

            // get information about the dimensions of the seleted printer+media.
            PrintDocumentImageableArea ia = null;
            var docWriter = PrintQueue.CreateXpsDocumentWriter(ref ia);

            if (docWriter != null && ia != null)
            {
                var paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;

                // Change the PageSize and PagePadding for the document to match the CanvasSize for
                // the printer device.
                paginator.PageSize = new Size(ia.MediaSizeWidth, ia.MediaSizeHeight);
                var t = new Thickness(72);  // copy.PagePadding;
                copy.PagePadding = new Thickness(
                    Math.Max(ia.OriginWidth, t.Left),
                    Math.Max(ia.OriginHeight, t.Top),
                    Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), t.Right),
                    Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), t.Bottom));

                copy.ColumnWidth = double.PositiveInfinity;
                //copy.PageWidth = 528; // allow the page to be the natural with of the output device

                // Send content to the printer.
                docWriter.Write(paginator);
            }
        }

    }
}

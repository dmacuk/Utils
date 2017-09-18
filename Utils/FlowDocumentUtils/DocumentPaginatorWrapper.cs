using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using JetBrains.Annotations;

namespace Utils.FlowDocumentUtils
{
    public class DocumentPaginatorWrapper : DocumentPaginator
    {
        readonly Size _pageSize;
        Size _margin;
        readonly DocumentPaginator _paginator;
        Typeface _typeface;
        /// <summary>  
        ///     Constructor por el cual se empieza a hacer el paginador  
        ///     para el FlowDocument.  
        /// </summary>  
        /// <param name="paginator" type="System.Windows.Documents.DocumentPaginator">  
        ///     <para>  
        ///         El paginador del documento, si se trata de un Flow document es posible hacer algo como:  
        ///         IDocumentPaginatorSource fuentePagina= miFlowDocument as IDocumentPaginatorSource;  
        ///         DocumentPaginator paginador = fuentePagina.DocumentPaginator  
        ///     </para>  
        /// </param>  
        /// <param name="pageSize" type="System.Windows.Size">  
        ///     <para>  
        ///         El tamaño de la hoja, 8*11.5 pulgadas es carta.  
        ///     </para>  
        /// </param>  
        /// <param name="margin" type="System.Windows.Size">  
        ///     <para>  
        ///         El Margen en las hojas  
        ///     </para>  
        /// </param>  
        public DocumentPaginatorWrapper(DocumentPaginator paginator, Size pageSize, Size margin)
        {
            _pageSize = pageSize;
            _margin = margin;
            _paginator = paginator;
            _paginator.PageSize = new Size(_pageSize.Width - margin.Width * 2,
                _pageSize.Height - margin.Height * 2);
        }
        //public DocumentPaginatorWrapper():base()  
        //{  
        //    _paginator = this;  
        //    _typeface = new Typeface("Arial");  
        //    PageSize=new Size(8*96, 96*11.5);  
        //    _margin = new Size(96, 48);  
        //}  
        /// <summary>  
        ///     Mueve un área en específico para generar encabezados o pies de página.  
        /// </summary>  
        /// <param name="rect" type="System.Windows.Rect">  
        ///     <para>  
        ///         Ubicación de este rectángulo.  
        ///     </para>  
        /// </param>  
        /// <returns>  
        ///     A System.Windows.Rect value...  
        /// </returns>  
        private Rect Move(Rect rect)
        {
            if (rect.IsEmpty)
            {
                return rect;
            }
            else
            {
                return new Rect(rect.Left + _margin.Width, rect.Top + _margin.Height,
                    rect.Width, rect.Height);
            }
        }
        /// <summary>  
        ///     Obtiene una página.  
        /// </summary>  
        /// <param name="pageNumber" type="int">  
        ///     <para>  
        ///         El npumero de página  
        ///     </para>  
        /// </param>  
        /// <returns>  
        ///     La página solicitada del documento.  
        /// </returns>  
        public override DocumentPage GetPage(int pageNumber)
        {
            var page = this != _paginator ? _paginator.GetPage(pageNumber) : new DocumentPage(new ContainerVisual());
            // Create a wrapper visual for transformation and add extras  
            ContainerVisual newpage = new ContainerVisual();
            DrawingVisual title = new DrawingVisual();

            using (DrawingContext ctx = title.RenderOpen())
            {
                ctx.DrawRectangle(Brushes.White, null, new Rect(page.Size));
                if (_typeface == null)
                {
                    _typeface = new Typeface("Arial");
                }
                FormattedText text = new FormattedText("Suprema Corte de Justicia de la Nación \tPágina " + (pageNumber + 1),
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    _typeface, 14, Brushes.Black);
                ctx.DrawText(text, new Point(0, 0)); // 1/4 inch above page content  
            }
            //DrawingVisual background = new DrawingVisual();  
            //using (DrawingContext ctx = background.RenderOpen())  
            //{  
            //    ctx.DrawRectangle(new SolidColorBrush(Color.FromRgb(255, 255, 255)), null, page.ContentBox);  
            //}  
            //newpage.Children.Add(background); // Scale down page and center  
            ContainerVisual smallerPage = new ContainerVisual();
            try
            {
                newpage.Children.Add(title);
                smallerPage.Children.Add(page.Visual);
                smallerPage.Transform = new MatrixTransform(0.95, 0, 0, 0.95,
                    0.025 * page.ContentBox.Width, 0.025 * page.ContentBox.Height);
                newpage.Children.Add(smallerPage);
                newpage.Transform = new TranslateTransform(_margin.Width, _margin.Height);
                return new DocumentPage(newpage, _pageSize, Move(page.BleedBox), Move(page.ContentBox));
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>  
        ///     Define si la cuenta de las páginas es o no válida  
        /// </summary>  
        /// <value>  
        ///     <para>  
        ///           
        ///     </para>  
        /// </value>  
        /// <remarks>  
        ///       
        /// </remarks>  
        public override bool IsPageCountValid => _paginator.IsPageCountValid;

        /// <summary>  
        ///     Obtiene la cuenta de las páginas  
        /// </summary>  
        /// <value>  
        ///     <para>  
        ///           
        ///     </para>  
        /// </value>  
        /// <remarks>  
        ///       
        /// </remarks>  
        public override int PageCount => _paginator.PageCount;

        public override Size PageSize
        {
            get => _paginator.PageSize;
            set
            {
                if (this == _paginator)
                {
                }
                else
                {
                    _paginator.PageSize = value;
                }
            }
        }

        public override IDocumentPaginatorSource Source => _paginator.Source;
    }

    [UsedImplicitly]
    public class IusPaginatedDocument : FlowDocument, IDocumentPaginatorSource
    {
        #region IDocumentPaginatorSource Members  

        private DocumentPaginator _documentPaginator;
        DocumentPaginator IDocumentPaginatorSource.DocumentPaginator
        {
            get
            {
                if (_documentPaginator != null)
                {
                    return _documentPaginator;
                }
                DocumentPaginator pgn = new IusDocumentPaginator(this);
                _documentPaginator = new DocumentPaginatorWrapper(pgn, new Size(96 * 11, 96 * 8.5), new Size(46, 96));
                return _documentPaginator;
            }
        }

        #endregion

    }
    public class IusDocumentPaginator : DocumentPaginator
    {
        readonly IDocumentPaginatorSource _iDocumentPaginatorSource;
        DocumentPage[] _page;
        public IusDocumentPaginator(FlowDocument doc)
        {
            var document = doc;
            var temporal = new FlowDocument
            {
                PageHeight = document.PageHeight,
                PageWidth = document.PageWidth,
                Background = Brushes.White
            };
            //temporal.  
            var list = doc.Blocks.ToList();
            foreach (var item in list)
            {
                temporal.Blocks.Add(item);
            }
            _iDocumentPaginatorSource = temporal;
        }
        public override DocumentPage GetPage(int pageNumber)
        {
            return _iDocumentPaginatorSource.DocumentPaginator.GetPage(pageNumber);
            //_page = new DocumentPage[iDocumentPaginatorSource.DocumentPaginator.PageCount];  
            //for (int i = 1; i < iDocumentPaginatorSource.DocumentPaginator.PageCount; i++)  
            //{  
            //    _page[i] = iDocumentPaginatorSource.DocumentPaginator.GetPage(i);  
            //}  
            //if (pageNumber > 0 && pageNumber < _page.Length)  
            //{  
            //    return _page[pageNumber];  
            //}  
            //else  
            //{  
            //    return null;  
            //}  
        }

        public override bool IsPageCountValid => true;

        public override int PageCount
        {
            get
            {
                _page = new DocumentPage[_iDocumentPaginatorSource.DocumentPaginator.PageCount];
                for (int i = 1; i < _iDocumentPaginatorSource.DocumentPaginator.PageCount; i++)
                {
                    _page[i] = _iDocumentPaginatorSource.DocumentPaginator.GetPage(i);
                }
                return _page.Length;
            }
        }

        private Size _pageSize;
        public override Size PageSize
        {
            get => _pageSize;
            set
            {
                _iDocumentPaginatorSource.DocumentPaginator.PageSize = value;
                _pageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source => _iDocumentPaginatorSource;
    }
}

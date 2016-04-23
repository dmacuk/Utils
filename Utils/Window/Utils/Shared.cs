using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Utils.Window.Utils
{
    public static class Shared
    {
        public static int CountWindows(string windowName)
        {
            var windows = Application.Current.Windows;
            return windows.Cast<System.Windows.Window>().Count(window => window.Name == windowName);
        }

        public static List<TChild> GetChildren<TChild>(DependencyObject reference) where TChild : Control
        {
            var result = new List<TChild>();

            // enumerate all of the children of the supplied element searching for all the
            // elements that match the supplied type
            for (var x = 0; x < VisualTreeHelper.GetChildrenCount(reference); x++)
            {
                var child = VisualTreeHelper.GetChild(reference, x);
                var tChildInstance = child as TChild;
                if (tChildInstance != null)
                {
                    result.Add(tChildInstance);
                }

                // now repeat the process on all the children of the current child element
                // by recursively calling this method
                result.AddRange(GetChildren<TChild>(child));
            }
            return result;
        }
    }
}
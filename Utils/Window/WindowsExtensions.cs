using Utils.Window.Utils;

namespace Utils.Window
{
    public static class WindowsExtensions
    {
        public static void SaveSettings(this System.Windows.Window window, bool saveChildren)
        {
            var saver = new WindowContentSaver(window, saveChildren);
            saver.Save();
        }

        public static void LoadSettings(this System.Windows.Window window, bool loadChildren)
        {
            var loader = new WindowContentLoader(window, loadChildren);
            loader.Load();
        }
    }
}
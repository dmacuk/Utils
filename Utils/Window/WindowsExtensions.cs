using Utils.Window.Utils;

namespace Utils.Window
{
    public static class WindowsExtensions
    {
        public static void SaveSettings(this System.Windows.Window window)
        {
            var saver = new WindowContentSaver(window);
            saver.Save();
        }

        public static void LoadSettings(this System.Windows.Window window)
        {
            var loader = new WindowContentLoader(window);
            loader.Load();
        }
    }
}
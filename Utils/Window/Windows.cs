namespace Utils.Window
{
    public static class Windows
    {
        public static void SetWindowSettings(this System.Windows.Window window, WindowSettings settings)
        {
            if (settings == null) return;
            window.Top = settings.Top;
            window.Left = settings.Left;
            window.Width = settings.Width;
            window.Height = settings.Height;
        }

        public static WindowSettings GetWindowSettings(this System.Windows.Window window)
        {
            return new WindowSettings
            {
                Top = window.Top,
                Left = window.Left,
                Width = window.Width,
                Height = window.Height
            };
        }
    }
}

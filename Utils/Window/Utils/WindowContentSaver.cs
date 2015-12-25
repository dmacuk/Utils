using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using Utils.Preference;
using Utils.Window.Utils.Grid;

namespace Utils.Window.Utils
{
    public class WindowContentSaver
    {
        private readonly System.Windows.Window _window;

        public WindowContentSaver(System.Windows.Window window)
        {
            _window = window;
        }

        public void Save()
        {
            var windowName = _window.Name;
            if (string.IsNullOrWhiteSpace(windowName))
            {
                Debug.WriteLine("Window has no Name");
                return;
            }
            var count = Shared.CountWindows(windowName);
            var windowNameCount = $"{windowName}[{count}]";
            var layout = GetWindowSettings(_window);
            PreferenceManager.SetPreference($"Layout.{windowNameCount}", layout);

            var controls = Shared.GetChildren<Control>(_window);
            foreach (var dataGrid in controls.OfType<DataGrid>())
            {
                SaveDataGrid(windowNameCount, dataGrid);
            }
        }

        private static void SaveDataGrid(string windowNameCount, DataGrid dataGrid)
        {
            var dataGridName = dataGrid.Name;
            if (string.IsNullOrWhiteSpace(dataGridName))
            {
                Debug.WriteLine("dataGrid has no Name");
                return;
            }
            var columnOptions = dataGrid.GetColumOptions();
            PreferenceManager.SetPreference($"DataGridColumnOption.{dataGridName}.{windowNameCount}", columnOptions);
        }

        private static WindowLayout GetWindowSettings(System.Windows.Window window)
        {
            return new WindowLayout
            {
                Top = window.Top,
                Left = window.Left,
                Width = window.Width,
                Height = window.Height,
                WindowState = window.WindowState
            };
        }
    }
}
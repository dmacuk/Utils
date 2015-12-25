using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using Utils.Preference;
using Utils.Window.Utils.Grid;

namespace Utils.Window.Utils
{
    internal class WindowContentLoader
    {
        private readonly System.Windows.Window _window;

        public WindowContentLoader(System.Windows.Window window)
        {
            _window = window;
        }

        public void Load()
        {
            var windowName = _window.Name;
            if (string.IsNullOrWhiteSpace(windowName))
            {
                Debug.WriteLine("Window has no Name");
                return;
            }
            var count = Shared.CountWindows(windowName);
            var windowNameCount = $"{windowName}[{count}]";
            var layout = PreferenceManager.GetPreference<WindowLayout>($"Layout.{windowNameCount}", null);
            SetWindowSettings(_window, layout);

            var controls = Shared.GetChildren<Control>(_window);
            foreach (var dataGrid in controls.OfType<DataGrid>())
            {
                LoadDataGrid(windowNameCount, dataGrid);
            }
        }

        private static void LoadDataGrid(string windowNameCount, DataGrid dataGrid)
        {
            var dataGridName = dataGrid.Name;
            if (string.IsNullOrWhiteSpace(dataGridName))
            {
                Debug.WriteLine("dataGrid has no Name");
                return;
            }
            var columnOptions =
                PreferenceManager.GetPreference<List<ColumnProperty>>(
                    $"DataGridColumnOption.{dataGridName}.{windowNameCount}", null);
            dataGrid.SetColumnOptions(columnOptions);
        }

        private static void SetWindowSettings(System.Windows.Window window, WindowLayout layout)
        {
            if (layout == null) return;
            window.Top = layout.Top;
            window.Left = layout.Left;
            window.Width = layout.Width;
            window.Height = layout.Height;
        }
    }
}
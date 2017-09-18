using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Utils.UserControls.DirectoryPicker
{
    /// <summary>
    /// Interaction logic for DirectoryPicker.xaml
    /// </summary>
    public sealed partial class DirectoryPicker : UserControl
    {

        public event DirectoryChangedHandler DirectoryChanged;

        private string _directory;

        public string Directory
        {
            get => _directory;
            set
            {
                if (_directory == value) return;
                _directory = value;
                DirectoryName.Text = _directory;
                OnDirectoryChanged();

            }
        }

        public DirectoryPicker()
        {
            InitializeComponent();
        }

        private void PickDirectory(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                DefaultDirectory = Directory
            };
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                Directory = dialog.FileName;
            }
        }

        private void OnDirectoryChanged()
        {
            DirectoryChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void DirectoryChangedHandler(object sender, EventArgs args);
}
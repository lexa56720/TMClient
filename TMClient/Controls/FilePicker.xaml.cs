using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMClient.Utils;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для FilePicker.xaml
    /// </summary>
    public partial class FilePicker : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder),
                                    typeof(string),
                                    typeof(FilePicker),
                                    new PropertyMetadata(string.Empty));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }


        public static readonly DependencyProperty FilePickedProperty =
        DependencyProperty.Register(nameof(FilePicked),
                                    typeof(ICommand),
                                    typeof(FilePicker),
                                    new PropertyMetadata(null));

        public ICommand FilePicked
        {
            get
            {
                return (ICommand)GetValue(FilePickedProperty);
            }
            set
            {
                SetValue(FilePickedProperty, value);
            }
        }

        public ICommand PathEntered => new Command(ExecuteCommand);

        public string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
        private string path;


        public event PropertyChangedEventHandler? PropertyChanged;

        public FilePicker()
        {
            InitializeComponent();
        }
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Explore();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Explore();
        }

        private void Explore()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png",
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                Multiselect = false,
            };
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                Path = dialog.FileName;

                ExecuteCommand();
            }
        }

        private void ExecuteCommand()
        {
            if (File.Exists(Path) && FilePicked != null && FilePicked.CanExecute(null))
                FilePicked.Execute(Path);
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для FileDisplay.xaml
    /// </summary>
    public partial class FileDisplay : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register(nameof(Path),
                                        typeof(string),
                                        typeof(FileDisplay),
                                        new PropertyMetadata(string.Empty, PathChanged));
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty IndexProperty =
        DependencyProperty.Register(nameof(Index),
                                typeof(int),
                                typeof(FileDisplay),
                                new PropertyMetadata(0));
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public bool IsImage
        {
            get => isImage;
            set
            {
                isImage = value;
                OnPropertyChanged(nameof(IsImage));
            }
        }
        private bool isImage;

        public ImageSource? Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        private ImageSource? image;

        public event PropertyChangedEventHandler? PropertyChanged;

        public FileDisplay()
        {
            InitializeComponent();
        }
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private static void PathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FileDisplay)d).PathChanged((string)e.NewValue);
        }

        private void PathChanged(string? newValue)
        {
            if (newValue == null || !System.IO.Path.Exists(newValue))
                return;
            IsImage = IsImagePath(newValue);
            if (IsImage)
                Image = new BitmapImage(new Uri(newValue));
            else
                Image = null;
        }
        private bool IsImagePath(string path)
        {
            var ext = System.IO.Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext))
                return false;

            return ext == ".png" || ext == ".jpg" || ext == ".jpeg";
        }
    }
}

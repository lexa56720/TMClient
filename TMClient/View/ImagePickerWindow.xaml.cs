using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TMClient.Controls;
using TMClient.ViewModel;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для ImagePickerWindow.xaml
    /// </summary>
    public partial class ImagePickerWindow : ModernWindow
    {
        public byte[] Image { get; set; } = [];
        public ImagePickerWindow(string path)
        {
            DataContext = new ImagePickerViewModel(path, DialogCompleted);
            Title = "Выбор изображения";
            InitializeComponent();
        }

        private void DialogCompleted(byte[] image)
        {
            Image = image;
            DialogResult = image.Length > 0;
            Close();
        }
    }
}

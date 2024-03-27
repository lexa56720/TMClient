using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TMClient.Controls;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для ImagePickerWindow.xaml
    /// </summary>
    public partial class ImagePickerWindow : ModernWindow
    {
        public string Img 
        { 
            get => img; 
            set => img = value; 
        }
        private string img;
        public ImagePickerWindow(string path)
        {
            Img = path;
            DataContext = this;
            InitializeComponent();
        }
    }
}

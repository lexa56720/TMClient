using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : UserControl, INotifyPropertyChanged
    {
        public double SplitterSize
        {
            get => splitterSize;
            set
            {
                splitterSize = value;
                OnPropertyChanged(nameof(SplitterSize));
            }
        }
        private double splitterSize;


        public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register(
            nameof(ImageSource),
            typeof(BitmapSource),
            typeof(ImagePicker),
            new PropertyMetadata(null,ImagePropertyChanged));

        private static void ImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ImagePicker)d).ImagePropertyChanged();
        }

        private void ImagePropertyChanged()
        {
            SizeSetup();
        }

        public BitmapSource ImageSource
        {
            get => (BitmapSource)GetValue(ImageSourceProperty);
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        public BitmapSource CroppedImage
        {
            get
            {
                if (ImageSource == null || !IsPickerLoaded)
                    return null;
                var width = (int)CenterColumn.Width.Value;
                var heigth = (int)CenterRow.Height.Value;
                var x = (int)(Center.X - CenterColumn.Width.Value / 2);
                var y = (int)(Center.Y - CenterRow.Height.Value / 2);
                var a = new Int32Rect(x, y, width, heigth);

                return new CroppedBitmap(ImageSource, a);
            }
        }

        public Point Center
        {
            get => center;
            set
            {
                center = value;
                OnPropertyChanged(nameof(Center));
            }
        }
        private Point center;

        public ImagePicker()
        {
            SplitterSize = 5;
            InitializeComponent();
        }

        private bool IsPickerLoaded = false;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GridSplitterDragDeltaX(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (CenterColumn.Width.Value > Image.ActualHeight - 20)
                CenterColumn.Width = new GridLength(Image.ActualHeight - 20);

            if (CenterColumn.Width.Value > Image.ActualWidth - 20)
                CenterColumn.Width = new GridLength(Image.ActualWidth - 20);

            CenterRow.Height = CenterColumn.Width;
            UpdateCenter();
        }

        private void GridSplitterDragDeltaY(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (CenterRow.Height.Value > Image.ActualHeight - 20)
                CenterRow.Height = new GridLength(Image.ActualHeight - 20);

            if (CenterRow.Height.Value > Image.ActualWidth - 20)
                CenterRow.Height = new GridLength(Image.ActualWidth - 20);


            CenterColumn.Width = CenterRow.Height;
            UpdateCenter();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            MoveCircle(sender, e);
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MoveCircle(sender, e);
        }
        private void MoveCircle(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(Image);
            var eventSource = Circle as UIElement;

            if (e.LeftButton == MouseButtonState.Pressed &&
                 mousePosition.X > SplitterSize + (Circle.ActualWidth / 2d) && mousePosition.X < Image.ActualWidth - (SplitterSize + (Circle.ActualWidth / 2d)) &&
                 mousePosition.Y > SplitterSize + (Circle.ActualHeight / 2d) && mousePosition.Y < Image.ActualHeight - (SplitterSize + (Circle.ActualHeight / 2d)))
            {
                var deltaX = e.GetPosition(eventSource).X - Circle.ActualWidth / 2d;
                var deltaY = e.GetPosition(eventSource).Y - Circle.ActualHeight / 2d;

                LeftSide.Width = new GridLength(MoreThanZero(LeftSide.ActualWidth + deltaX), GridUnitType.Star);
                RightSide.Width = new GridLength(MoreThanZero(RightSide.ActualWidth - deltaX), GridUnitType.Star);

                TopSide.Height = new GridLength(MoreThanZero(TopSide.ActualHeight + deltaY), GridUnitType.Star);
                BottomSide.Height = new GridLength(MoreThanZero(BottomSide.ActualHeight - deltaY), GridUnitType.Star);

                UpdateCenter();
            }
        }

        private void UpdateCenter()
        {
            Point center = Circle.TransformToAncestor(Root).Transform(new Point(0, 0));
            Center = new Point(center.X + Circle.ActualWidth / 2d, center.Y + Circle.ActualHeight / 2d);
            OnPropertyChanged(nameof(CroppedImage));
        }

        private double MoreThanZero(double value)
        {
            if (value < 0)
                return 0;
            return value;
        }

        private void SizeSetup()
        {
            IsPickerLoaded = true;

            if (ImageSource is not BitmapImage image)
            {
                Root.MinWidth = 200;
                Root.MinHeight = 200;
                Center = new Point(200, 200);
                return;
            }

            Root.MaxWidth = image.PixelWidth;
            Root.MaxHeight = image.PixelHeight;
            Root.MinWidth = image.PixelWidth;
            Root.MinHeight = image.PixelHeight;

            CenterRow.Height =new GridLength( 200);
            CenterColumn.Width = new GridLength(200);

            LeftSide.Width = new GridLength(1, GridUnitType.Star);
            RightSide.Width = new GridLength(1, GridUnitType.Star);

            TopSide.Height = new GridLength(1, GridUnitType.Star);
            BottomSide.Height = new GridLength(1, GridUnitType.Star);

            Rect.Rect = new Rect(0, 0, image.PixelWidth, image.PixelHeight);
            Center = new Point(image.PixelWidth / 2, image.PixelHeight / 2);
            OnPropertyChanged(nameof(CroppedImage));
        }
    }
}

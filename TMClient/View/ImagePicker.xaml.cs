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

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : ModernWindow, INotifyPropertyChanged
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


        public ImagePicker(string path)
        {
            var info = SixLabors.ImageSharp.Image.Identify(path);
            DataContext = this;
            InitializeComponent();
            SplitterSize = 5;
            Image.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            window.Width = info.Width;
            window.Height = info.Height;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GridSplitterDragDeltaX(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (CenterColumn.Width.Value > Image.ActualHeight)
                CenterColumn.Width = new GridLength(Image.ActualHeight);

            if (CenterColumn.Width.Value > Image.ActualWidth)
                CenterColumn.Width = new GridLength(Image.ActualWidth);

            CenterRow.Height = CenterColumn.Width;
            UpdateCenter();
        }

        private void GridSplitterDragDeltaY(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (CenterRow.Height.Value > Image.ActualHeight)
                CenterRow.Height = new GridLength(Image.ActualHeight);

            if (CenterRow.Height.Value > Image.ActualWidth)
                CenterRow.Height = new GridLength(Image.ActualWidth);


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
            Center= new Point(center.X + Circle.ActualWidth / 2d, center.Y + Circle.ActualHeight / 2d);
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCenter();
        }

        private double MoreThanZero(double value)
        {
            if(value<0)
                return 0;
            return value;
        }
    }
}

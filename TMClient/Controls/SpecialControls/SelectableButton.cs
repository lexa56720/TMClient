using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Reflection;

namespace TMClient.Controls
{
    class SelectableButton : IconButton
    {
        private Color DefaultBackgroundColor = (Color)ColorConverter.ConvertFromString("#00383838");

        private Color BackgroundColor = (Color)ColorConverter.ConvertFromString("#00383838");

        private Color ClickBackgroundColor = (Color)ColorConverter.ConvertFromString("#80545454");

        private Color SelectedColor = (Color)ColorConverter.ConvertFromString("#80383838");

        private Color BorderColor = (Color)ColorConverter.ConvertFromString("#3498DB");


        private Border? Border;

        public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register(nameof(IsSelected),
                                    typeof(bool),
                                    typeof(SelectableButton),
                                    new FrameworkPropertyMetadata(
            false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,IsSelectedChanged));

        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SelectableButton)d).IsSelectedChanged((bool)e.NewValue);
        }

        private void IsSelectedChanged(bool newValue)
        {
            if (newValue == true)
            {
                ShowBorder();
                BackgroundColor = SelectedColor;
            }
            else
            {
                HideBorder();
                BackgroundColor = DefaultBackgroundColor;
            }
            GetMouseLeaveAnimation().Begin(this);
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public SelectableButton() : base()
        {
            Style = (Style)FindResource("ButtonTemplate");
            Background = new SolidColorBrush(DefaultBackgroundColor);
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Border != null)
            {
                Border.Margin = new(-ActualWidth, 0, 0, 0);
                Border.Height = ActualHeight / 2;
            }
        }

        private void ShowBorder()
        {
            if (Border == null)
            {
                Border = new Border()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Height = ActualHeight / 2,
                    Width = 5,
                    Margin = new(-ActualWidth, 0, 0, 0),
                    Background = new SolidColorBrush(BorderColor),
                    CornerRadius = new(2)
                };

                mainGrid.Children.Insert(mainGrid.Children.Count, Border);
            }

            GetBorderShowAnimation().Begin(Border);
        }

        private void HideBorder()
        {
            GetBorderHideAnimation().Begin(Border);
        }

        private Storyboard GetMouseEnterAnimation()
        {
            var mouseEnterColorAnimation = new ColorAnimation
            {
                To = SelectedColor,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };
            Storyboard.SetTargetProperty(mouseEnterColorAnimation, new PropertyPath("(Grid.Background).(SolidColorBrush.Color)", null));
            Storyboard mouseEnterStoryboard = new Storyboard();
            mouseEnterStoryboard.Children.Add(mouseEnterColorAnimation);
            return mouseEnterStoryboard;
        }
        private Storyboard GetMouseLeaveAnimation()
        {
            var mouseLeaveColorAnimation = new ColorAnimation
            {
                To = BackgroundColor,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };
            Storyboard.SetTargetProperty(mouseLeaveColorAnimation, new PropertyPath("(Grid.Background).(SolidColorBrush.Color)", null));
            Storyboard mouseLeaveStoryboard = new Storyboard();
            mouseLeaveStoryboard.Children.Add(mouseLeaveColorAnimation);
            return mouseLeaveStoryboard;
        }
        private Storyboard GetClickAnimation()
        {
            var clickColorAnimation = new ColorAnimation
            {
                To = ClickBackgroundColor,
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                AutoReverse = true,
                FillBehavior = FillBehavior.HoldEnd
            };
            Storyboard.SetTargetProperty(clickColorAnimation, new PropertyPath("(Grid.Background).(SolidColorBrush.Color)", null));
            Storyboard clickStoryboard = new Storyboard();
            clickStoryboard.Children.Add(clickColorAnimation);
            return clickStoryboard;
        }
        private Storyboard GetBorderShowAnimation()
        {
            var mouseLeaveColorAnimation = new DoubleAnimation
            {
                From = 0,
                To = ActualHeight / 2,
                AccelerationRatio = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            Storyboard.SetTargetProperty(mouseLeaveColorAnimation, new PropertyPath("Height", null));
            Storyboard mouseLeaveStoryboard = new Storyboard();
            mouseLeaveStoryboard.Children.Add(mouseLeaveColorAnimation);
            return mouseLeaveStoryboard;
        }
        private Storyboard GetBorderHideAnimation()
        {
            var mouseLeaveColorAnimation = new DoubleAnimation
            {
                To = 0,
                AccelerationRatio = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            Storyboard.SetTargetProperty(mouseLeaveColorAnimation, new PropertyPath("Height", null));
            Storyboard mouseLeaveStoryboard = new Storyboard();
            mouseLeaveStoryboard.Children.Add(mouseLeaveColorAnimation);
            return mouseLeaveStoryboard;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            GetMouseEnterAnimation().Begin(this);
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            GetMouseLeaveAnimation().Begin(this);
            base.OnMouseEnter(e);
        }

        protected override void OnClick()
        {
            IsSelected = true;
            GetClickAnimation().Begin(this);

            base.OnClick();
        }
    }
}

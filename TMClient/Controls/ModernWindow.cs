using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using TMClient.Utils;

namespace TMClient.Controls
{
    public class ModernWindow : Window
    {
        private UserControl AppContent = new UserControl();
        private Grid MainGrid = new Grid();
        private Border MainBorder = new Border();
        private TextBlock TitleText = new TextBlock() { Text = "TM CLIENT" };
        private Grid TitleGrid = new Grid();
        private StackPanel IconPanel = new StackPanel();
        private StackPanel WindowButtons = new StackPanel();


        public Button MinimizeButton { get; private set; }
        public IconButton MaximizeButton { get; private set; }
        public IconButton RestoreButton { get; private set; }
        public IconButton CloseButton { get; private set; }


        public new string Title
        {
            get => title;
            set
            {
                title = value;
                TitleText.Text = title;
            }
        }
        private string title;


        private WindowChrome Chrome = new WindowChrome()
        {
            CaptionHeight = 34,
            CornerRadius = new CornerRadius(5),
            GlassFrameThickness = new Thickness(0),
        };

        public ModernWindow()
        {
            InitializeComponent();
            EventSetup();
        }

        private void EventSetup()
        {
            this.StateChanged += MainWindowStateChangeRaised;
        }

        private void InitializeComponent()
        {
            WindowChrome.SetWindowChrome(this, Chrome);
            MainBorder.BorderThickness = new Thickness();
            MainBorder.Child = MainGrid;
            MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });


            MainGrid.Children.Add(TitleGrid);
            MainGrid.Children.Add(AppContent);
            Grid.SetRow(TitleGrid, 0);
            Grid.SetRow(AppContent, 1);


            TitleGrid.Children.Add(IconPanel);
            TitleGrid.Children.Add(WindowButtons);


            SetupTitle();
            Content = MainBorder;
        }

        private void SetupTitle()
        {
            IconPanel.Children.Add(TitleText);
            TitleText.VerticalAlignment = VerticalAlignment.Center;
            TitleText.Margin = new Thickness(0, 5, 0, 0);

            MinimizeButton = new Button()
            {
                Content = "\uE949",
                Style = (Style)FindResource("CaptionButtonStyle"),
                Command = new Command((o) => SystemCommands.MinimizeWindow(this))
            };
            MaximizeButton = new IconButton()
            {
                Content = "\uE739",
                Visibility = Visibility.Visible,
                Style = (Style)FindResource("CaptionButtonStyle"),
                Command = new Command((o) => SystemCommands.MaximizeWindow(this))
            };
            RestoreButton = new IconButton()
            {
                Content = "\uE923",
                Visibility = Visibility.Collapsed,
                Style = (Style)FindResource("CaptionButtonStyle"),
                Command = new Command((o) => SystemCommands.RestoreWindow(this))
            };
            CloseButton = new IconButton()
            {
                Content = "\ue8bb",
                Style = (Style)FindResource("CloseButtonStyle"),
                Command = new Command((o) => SystemCommands.CloseWindow(this))
            };

            WindowChrome.SetIsHitTestVisibleInChrome(MinimizeButton, true);
            WindowChrome.SetIsHitTestVisibleInChrome(MaximizeButton, true);
            WindowChrome.SetIsHitTestVisibleInChrome(RestoreButton, true);
            WindowChrome.SetIsHitTestVisibleInChrome(CloseButton, true);

            WindowButtons.Children.Add(MinimizeButton);
            WindowButtons.Children.Add(RestoreButton);
            WindowButtons.Children.Add(MaximizeButton);
            WindowButtons.Children.Add(CloseButton);

            WindowButtons.Orientation = Orientation.Horizontal;
            WindowButtons.HorizontalAlignment = HorizontalAlignment.Right;
        }

        private void MainWindowStateChangeRaised(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (newContent != MainBorder)
            {
                base.OnContentChanged(oldContent, newContent);
                AppContent.Content = newContent;

                Content = MainBorder;
            }
        }
    }
}

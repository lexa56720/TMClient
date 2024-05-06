using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using TMClient.Utils;

namespace TMClient.Controls
{

    public class ModernWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, 
                                                                ref WindowCompositionAttributeData data);

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute,
                                                         ref uint pvAttribute, uint cbAttribute);
        private enum AccentState
        {
            ACCENT_DISABLED = 1,
            ACCENT_ENABLE_GRADIENT = 0,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
            ACCENT_INVALID_STATE = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        private enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        // Copied from dwmapi.h
        private enum DWMWINDOWATTRIBUTE
        {
            DWMWA_WINDOW_CORNER_PREFERENCE = 33,
            DWMWA_BORDER_COLOR = 34,
        }

        // Copied from dwmapi.h
        private enum DWM_WINDOW_CORNER_PREFERENCE
        {
            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND = 1,
            DWMWCP_ROUND = 2,
            DWMWCP_ROUNDSMALL = 3
        }


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
            ResizeBorderThickness = new Thickness(5),
            GlassFrameThickness = new Thickness(0),
            UseAeroCaptionButtons = false,
        };

        static ModernWindow()
        {
            ResizeModeProperty.AddOwner(typeof(ModernWindow), new FrameworkPropertyMetadata(ResizeMode.CanResize, ResizeModeChanged));
        }
        public ModernWindow()
        {
            InitializeComponent();
            EventSetup();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private static void ResizeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not ResizeMode mode)
                return;

            (d as ModernWindow)?.ResizeModeChanged(mode);
        }

        private void ResizeModeChanged(ResizeMode mode)
        {
            if (mode == ResizeMode.NoResize)
                MaximizeButton.Visibility = Visibility.Collapsed;
            else
                MaximizeButton.Visibility = Visibility.Visible;
        }

        protected override void OnClosed(EventArgs e)
        {
            StateChanged -= MainWindowStateChangeRaised;
            Loaded -= Window_Loaded;
            base.OnClosed(e);
        }
        private void EventSetup()
        {
            StateChanged += MainWindowStateChangeRaised;
            Loaded += Window_Loaded;
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
            TitleText.Margin = new Thickness(0, 2, 0, 0);

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
                Command = new Command((o) =>
                {
                    MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                    SystemCommands.MaximizeWindow(this);
                })
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
                if (ResizeMode != ResizeMode.NoResize)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
            if (Environment.OSVersion.Version.Build >= 22000)
            {
                EnableRoundCorners();
                EnableBorderless();
            }
        }
        private void EnableBorderless()
        {
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR;
            var preference = 0xFFFFFFFE;
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
        }
        private void EnableRoundCorners()
        {
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = (uint)DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
        }
        private void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);
            var accent = new AccentPolicy
            {
                AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND,
            };

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }
    }
}

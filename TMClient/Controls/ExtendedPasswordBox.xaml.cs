using ClientApiWrapper.Types;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для ExtendedPasswordBox.xaml
    /// </summary>
    public partial class ExtendedPasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register(nameof(Password),
                                    typeof(string),
                                    typeof(ExtendedPasswordBox),
                                    new PropertyMetadata(null, PasswordChanged));

        private static void PasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = ((ExtendedPasswordBox)d).passbox;
            if (!passwordBox.Password.Equals((string)e.NewValue))
                passwordBox.Password = (string)e.NewValue;
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set
            {
                SetValue(PasswordProperty, value);
            }
        }
        public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder),
                                    typeof(string),
                                    typeof(ExtendedPasswordBox),
                                    new PropertyMetadata(string.Empty));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        public ExtendedPasswordBox()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = passbox.Password;
        }
    }
}

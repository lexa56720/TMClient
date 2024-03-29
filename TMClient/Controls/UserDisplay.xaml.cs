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
    /// Логика взаимодействия для UserDisplay.xaml
    /// </summary>
    public partial class UserDisplay : UserControl
    {
        public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register(nameof(User),
                                    typeof(User),
                                    typeof(UserDisplay),
                                    new PropertyMetadata(null));
        public User User
        {
            get => (User)GetValue(UserProperty);
            set
            {
                SetValue(UserProperty, value);
            }
        }
        public UserDisplay()
        {
            InitializeComponent();
        }
    }
}

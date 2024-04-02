using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Controls
{
    using ClientApiWrapper.Types;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
        /// Логика взаимодействия для SelectableButton.xaml
        /// </summary>
        public partial class SelectableButton : IconButton
        {
            public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected),
                                        typeof(bool),
                                        typeof(SelectableButton),
                                        new PropertyMetadata(false));

            public event PropertyChangedEventHandler? PropertyChanged;

            public bool IsSelected
            {
                get => (bool)GetValue(IsSelectedProperty);
                set
                {
                    SetValue(IsSelectedProperty, value);
                }
            }

            public SelectableButton()
            {
                InitializeComponent();
                Style = (Style)FindResource("ButtonTemplate");
            }

            private void IconButton_Click(object sender, RoutedEventArgs e)
            {
                IsSelected = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
    }

}

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
using TMClient.Utils;
using System.Diagnostics.CodeAnalysis;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для SystemMessageControl.xaml
    /// </summary>
    public partial class SystemMessageControl : MessageBaseControl
    {

        [SetsRequiredMembers]
        public SystemMessageControl(SystemMessage message):base(message)
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}

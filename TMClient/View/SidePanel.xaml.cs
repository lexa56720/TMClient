using System.Windows.Controls;
using TMClient.ViewModel;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для SidePanel.xaml
    /// </summary>
    public partial class SidePanel : Page
    {
        public SidePanel()
        {
            InitializeComponent();
            DataContext = new SidePanelViewModel();
        }
    }
}

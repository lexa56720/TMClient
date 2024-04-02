using System.Windows.Controls;
using TMClient.ViewModel;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для SidePanel.xaml
    /// </summary>
    public partial class SidePanel : Page
    {
        private readonly SidePanelViewModel ViewModel;
        public SidePanel()
        {
            InitializeComponent();
            ViewModel = new SidePanelViewModel();
            DataContext = ViewModel; 
        }

        public void UnselectAllChats()
        {
            ViewModel.SelectedChat=null;
            ViewModel.SelectedFriend=null;
        }
    }
}

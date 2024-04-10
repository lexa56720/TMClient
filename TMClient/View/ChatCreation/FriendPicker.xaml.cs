using ClientApiWrapper.Interfaces;
using System.Windows;
using System.Windows.Controls;
using TMClient.Controls;
using TMClient.ViewModel;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для FriendPicker.xaml
    /// </summary>
    public partial class FriendPicker : Page
    {

        public FriendPicker(Action<User[]> NextPage)
        {
            DataContext = new FriendPickerViewModel(NextPage);
            InitializeComponent();
        }

    }
}

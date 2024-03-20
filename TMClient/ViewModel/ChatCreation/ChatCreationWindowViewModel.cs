using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel
{
    internal class ChatCreationWindowViewModel : BaseViewModel
    {
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        private Page currentPage = null!;

        public Visibility NavigationVisibility
        {
            get => navigationVisibility;
            set
            {
                navigationVisibility = value;
                OnPropertyChanged(nameof(NavigationVisibility));
            }
        }
        private Visibility navigationVisibility=Visibility.Collapsed;

        public ICommand BackCommand => new Command(BackNavigation);


        private readonly Page FriendPicker;
        private readonly Action<Chat?> DialogCompleted;

        public ChatCreationWindowViewModel(Action<Chat?> dialogCompleted)
        {
            FriendPicker = new FriendPicker(NextPage);
            CurrentPage = FriendPicker;
            DialogCompleted = dialogCompleted;
        }

        private void NextPage(User[] users)
        {
            NavigationVisibility = Visibility.Visible;
            CurrentPage = new ChatNamePicker(users, DialogCompleted);
        }

        private void BackNavigation()
        {
            NavigationVisibility = Visibility.Collapsed;
            CurrentPage = FriendPicker;
        }
    }
}

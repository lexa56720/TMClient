using ApiWrapper.Interfaces;
using ApiWrapper.Types;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;
using TMClient.View;
using TMClient.View.Auth;

namespace TMClient.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public Page SidePanelFrame
        {
            get => sidePanelFrame;
            set
            {
                sidePanelFrame = value;
                OnPropertyChanged(nameof(SidePanelFrame));
            }
        }
        private Page sidePanelFrame = null!;

        public ICommand ChangeSideBarState => new Command(SwitchSideBarState);

        public ICommand ProfileCommand => new Command(() => MainFrame = Profile);
        public ICommand NotificationCommand => new Command(() => MainFrame = Notifications);
        public ICommand SettingsCommand => new Command(() => MainFrame = Settings);
        public ICommand LogoutCommand => new Command(Logout);
        public ICommand ChatCommand => new Command(() => MainFrame = ChatPage);

        public int NotificationCount
        {
            get => notificationCount;
            set
            {
                notificationCount = value;
                OnPropertyChanged(nameof(NotificationCount));
            }
        }
        private int notificationCount;

        public Visibility SideBarState
        {
            get => sideBarState;
            set
            {
                sideBarState = value;
                OnPropertyChanged(nameof(SideBarState));
            }
        }
        private Visibility sideBarState = Visibility.Collapsed;

        public Page MainFrame
        {
            get => mainFrame;
            set
            {
                mainFrame = value;
                OnPropertyChanged(nameof(MainFrame));
            }
        }
        private Page mainFrame = null!;

        public bool IsInModalMode
        {
            get => isInModalMode;
            set
            {
                isInModalMode = value;
                OnPropertyChanged(nameof(IsInModalMode));
            }
        }
        private bool isInModalMode;

        private readonly SidePanel Panel = new();
        private readonly Settings Settings = new();
        private readonly Profile Profile = new();
        private readonly Notifications Notifications = new();
        private Page ChatPage = new();

        public MainViewModel()
        {
            ChatPage = new ChatView(new Chat(0, "намба ван чат", false));
            MainFrame = ChatPage;
            SidePanelFrame = Panel;

            Messenger.Subscribe<Page>(Messages.OpenChatPage, (o, p) =>
            {
                ChatPage = p;
                MainFrame = p;
            });

            Messenger.Subscribe(Messages.ModalOpened, () => IsInModalMode = true);
            Messenger.Subscribe(Messages.ModalClosed, () => IsInModalMode = false);

            CurrentUser.ChatInvites.CollectionChanged += ChatInvitesChanged;
            CurrentUser.FriendRequests.CollectionChanged += FriendRequests_CollectionChanged;
        }

        private void FriendRequests_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateNotificationCount();
        }

        private void ChatInvitesChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateNotificationCount();
        }

        private void UpdateNotificationCount()
        {
            NotificationCount = CurrentUser.ChatInvites.Count + CurrentUser.FriendRequests.Count;
        }

        private void SwitchSideBarState()
        {
            if (SideBarState == Visibility.Collapsed)
                SideBarState = Visibility.Visible;
            else
                SideBarState = Visibility.Collapsed;
        }


        private void Logout()
        {

            Messenger.Send(Messages.CloseMainWindow);
            ((App)App.Current).Logout();
        }
    }
}

using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Utils;
using TMClient.View;

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

        public bool IsProfileSelected
        {
            get => isProfileSelected;
            set
            {
                isProfileSelected = value;
                if (value)
                    UnselectAllExcept(nameof(IsProfileSelected));
                OnPropertyChanged(nameof(IsProfileSelected));
            }
        }
        private bool isProfileSelected;

        public bool IsNotificationsSelected
        {
            get => isNotificationsSelected;
            set
            {
                isNotificationsSelected = value; 
                if (value)
                    UnselectAllExcept(nameof(IsNotificationsSelected));
                OnPropertyChanged(nameof(IsNotificationsSelected));
            }
        }
        private bool isNotificationsSelected;

        public bool IsChatSelected
        {
            get => isChatSelected;
            set
            {
                isChatSelected = value;
                if (value)
                    UnselectAllExcept(nameof(IsChatSelected));
                OnPropertyChanged(nameof(IsChatSelected));
            }
        }
        private bool isChatSelected;

        public bool IsSettingsSelected
        {
            get => isSettingsSelected;
            set
            {
                isSettingsSelected = value;
                if (value)
                    UnselectAllExcept(nameof(IsSettingsSelected));
                OnPropertyChanged(nameof(IsSettingsSelected));
            }
        }
        private bool isSettingsSelected;

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
        public bool IsLoading
        {
            get => isLoading;
            private set
            {
                isLoading = value;
                if (value)
                    LoadingVisibility = Visibility.Visible;
                else
                    LoadingVisibility = Visibility.Collapsed;

                OnPropertyChanged(nameof(IsLoading));
            }
        }
        private bool isLoading;
        public Visibility LoadingVisibility
        {
            get => loadingVisibility;
            set
            {
                loadingVisibility = value;
                OnPropertyChanged(nameof(LoadingVisibility));
            }
        }
        private Visibility loadingVisibility = Visibility.Collapsed;


        private readonly SidePanel Panel = new();
        private readonly Settings Settings = new();
        private readonly Profile Profile = new();
        private readonly Notifications Notifications = new();
        private Page? ChatPage;


        public MainViewModel()
        {
            SidePanelFrame = Panel;

            Messenger.Subscribe<Page>(Messages.OpenChatPage, (o, p) =>
            {
                ChatPage = p;
                MainFrame = p;
                IsChatSelected = true;
            });

            Messenger.Subscribe(Messages.ModalOpened, () => IsInModalMode = true);
            Messenger.Subscribe(Messages.ModalClosed, () => IsInModalMode = false);

            Messenger.Subscribe(Messages.LoadingStart, () => IsLoading = true);
            Messenger.Subscribe(Messages.LoadingOver, () => IsLoading = false);

            CurrentUser.ChatInvites.CollectionChanged += ChatInvitesChanged;
            CurrentUser.FriendRequests.CollectionChanged += FriendRequestsChanged;
            UpdateNotificationCount();
        }

        private void FriendRequestsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateNotificationCount();
        }

        private void ChatInvitesChanged(object? sender, NotifyCollectionChangedEventArgs e)
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

        private void UnselectAllExcept(string name)
        {
            if (nameof(IsProfileSelected) != name)
                IsProfileSelected = false;
            if (nameof(IsNotificationsSelected) != name)
                IsNotificationsSelected = false;
            if (nameof(IsChatSelected) != name)
                IsChatSelected = false;
            if (nameof(IsSettingsSelected) != name)
                IsSettingsSelected = false;
        }
        private void Logout()
        {
            CurrentUser.ChatInvites.CollectionChanged -= ChatInvitesChanged;
            CurrentUser.FriendRequests.CollectionChanged -= FriendRequestsChanged;
            Messenger.Send(Messages.CloseMainWindow);
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            ((App)Application.Current).Logout();
        }
    }
}

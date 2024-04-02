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
        public ICommand ProfileCommand => new Command(() => OpenNonChatPage(Profile));
        public ICommand NotificationCommand => new Command(() => OpenNonChatPage(Notifications));
        public ICommand SettingsCommand => new Command(() => OpenNonChatPage(Settings));
        public ICommand LogoutCommand => new Command(Logout);

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

        private int ModalCount
        {
            get => modalCount;
            set
            {
                modalCount = value;
                if (value == 0 && IsInModalMode != false)
                    IsInModalMode = false;
                else if (value > 0 && IsInModalMode != true)
                    IsInModalMode = true;
            }
        }
        private int modalCount;

        public MainViewModel()
        {
            SidePanelFrame = Panel;

            Messenger.Subscribe<Page>(Messages.OpenChatPage, (o, page) =>
            {
                MainFrame = page;
                UnselectAllExcept(string.Empty);
            });

            Messenger.Subscribe(Messages.ModalOpened, () => ModalCount++);
            Messenger.Subscribe(Messages.ModalClosed, () => ModalCount--);

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

        private void OpenNonChatPage(Page page)
        {
            MainFrame = page;
            Panel.UnselectAllChats();
        }
        private void UnselectAllExcept(string name)
        {
            if (nameof(IsProfileSelected) != name)
                IsProfileSelected = false;
            if (nameof(IsNotificationsSelected) != name)
                IsNotificationsSelected = false;
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

﻿using ApiWrapper.Interfaces;
using ApiWrapper.Types;
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
        private Page sidePanelFrame;

        public ICommand ChangeSideBarState => new Command(SwitchSideBarState);

        public ICommand ProfileCommand => new Command(() => MainFrame = Profile);
        public ICommand NotificationCommand => new Command(() => MainFrame = Notifications);
        public ICommand SettingsCommand => new Command(() => MainFrame = Settings);
        public ICommand LogoutCommand => new Command(Logout);
        public ICommand ChatCommand => new Command(() => MainFrame = ChatPage);


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
        private Page mainFrame;

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

        private SidePanel Panel = new();
        private Settings Settings = new();
        private Profile Profile = new();
        private Notifications Notifications = new();
        private Page ChatPage = new();
        public MainViewModel()
        {


            ChatPage = new ChatView(new Chat(0, "намба ван чат",false));
            MainFrame = ChatPage;
            SidePanelFrame = Panel;

            Messenger.Subscribe<Page>(Messages.OpenChatPage, (o, p) =>
            {
                ChatPage = p;
                MainFrame = p;
            });

            Messenger.Subscribe(Messages.ModalOpened, () => IsInModalMode = true);
            Messenger.Subscribe(Messages.ModalClosed, () => IsInModalMode = false);
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

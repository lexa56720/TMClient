﻿using ApiWrapper.Interfaces;
using AsyncAwaitBestPractices.MVVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model.Auth;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Auth
{
    class MainAuthViewModel : BaseAuthViewModel
    {
        public Page EnteringFrame
        {
            get => enteringFrame;
            set
            {
                enteringFrame = value;
                OnPropertyChanged(nameof(EnteringFrame));
            }
        }
        private Page enteringFrame = null!;

        private readonly Page Registration;
        private readonly Page Auth;

        public bool IsLoginPage
        {
            get => isLoginPage;
            set
            {
                isLoginPage = value;
                OnPropertyChanged(nameof(IsLoginPage));
            }
        }
        private bool isLoginPage = true;

        public ICommand Switch => new Command(SwitchPages);

        public ICommand WindowLoaded => new AsyncCommand(TryToLoadApi);

        public bool IsLoaded
        {
            get => isLoaded;
            set
            {
                isLoaded = value;
                if (value)
                    LoadingVisibility = Visibility.Hidden;
                else
                    LoadingVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }
        private bool isLoaded = true;

        public Visibility LoadingVisibility
        {
            get => loadingVisibility;
            set
            {
                loadingVisibility = value;
                OnPropertyChanged(nameof(LoadingVisibility));
            }
        }
        private Visibility loadingVisibility = Visibility.Hidden;

        public MainAuthViewModel(Func<IApi?, bool> returnApi) : base(returnApi)
        {
            Registration = new SignUpView(returnApi);
            Auth = new AuthView(returnApi);

            EnteringFrame = Auth;

            Messenger.Subscribe(Messages.AuthLoadingStart,
                () => Application.Current.Dispatcher.Invoke(() => IsLoaded = false));

            Messenger.Subscribe(Messages.AuthLoadingFinish,
                () => Application.Current.Dispatcher.Invoke(() => IsLoaded = true));
        }

        private async Task TryToLoadApi()
        {       
            if (Preferences.Default.IsSaveAuth != false)
            {
                IApi? api = await AuthModel.TryGetApi();
                ReturnApi.Invoke(api);
            }    
        }


        private void SwitchPages()
        {
            if (IsLoginPage)
                EnteringFrame = Registration;
            else
                EnteringFrame = Auth;
            IsLoginPage = !IsLoginPage;
        }
    }
}

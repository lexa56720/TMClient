using ApiWrapper.Interfaces;
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

        private Page Registration { get; } = new SignUpView();
        private Page Auth { get; } = new AuthView();

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


        public MainAuthViewModel()
        {
            EnteringFrame = Auth;

            Messenger.Subscribe(Messages.AuthLoadingStart,
                () => Application.Current.Dispatcher.Invoke(() => IsLoaded = false));
            Messenger.Subscribe(Messages.AuthLoadingFinish,
                () => Application.Current.Dispatcher.Invoke(() => IsLoaded = true));
        }

        private async Task TryToLoadApi()
        {
            if (App.IsAutoLogin == false)
            {
                IsLoaded = true;
                return;
            }
               
            IsLoaded = false;
            IApi? api = await AuthModel.TryGetApi();
            if (api != null)
            {
                IsLoaded = true;
                await OpenMainWindow(api);
                return;
            }

            IsLoaded = true;
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

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
    class MainAuthViewModel : BaseAuthViewModel<MainAuthModel>
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
        private readonly Page Settings;
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

        public ICommand SwitchPage => new Command(SwitchPages);
        public ICommand WindowLoaded => new AsyncCommand(TryToLoadApi);
        public ICommand WindowClosed => new Command(Dispose);

        public ICommand BackNavigation => new Command(OpenPreviousPage);

        public Visibility SwitchPageVisibility
        {
            get => switchPageVisibility;
            set
            {
                switchPageVisibility = value;
                OnPropertyChanged(nameof(SwitchPageVisibility));
            }
        }
        private Visibility switchPageVisibility;

        public Visibility BackNavigationVisibility
        {
            get => backNavigationVisibility;
            set
            {
                backNavigationVisibility = value;
                OnPropertyChanged(nameof(BackNavigationVisibility));
            }
        }
        private Visibility backNavigationVisibility=Visibility.Hidden;


        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                if (value)
                    LoadingVisibility = Visibility.Visible;
                else
                    LoadingVisibility = Visibility.Hidden;
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
        private Visibility loadingVisibility = Visibility.Hidden;

        private Page? PreviousPage;

        public MainAuthViewModel(Func<IApi?, bool> returnApi) : base(returnApi)
        {
            Registration = new SignUpView(returnApi);
            Auth = new AuthView(returnApi);
            Settings = new Settings();

            EnteringFrame = Auth;

            Messenger.Subscribe(Messages.OpenSettingsPage,OpenSettings);
            Messenger.Subscribe(Messages.AuthLoadingStart, LoadingStart);
            Messenger.Subscribe(Messages.AuthLoadingFinish, LoadingOver);
        }

        private void Dispose()
        {
            Messenger.Unsubscribe(Messages.OpenSettingsPage, OpenSettings);
            Messenger.Unsubscribe(Messages.AuthLoadingStart, LoadingStart);
            Messenger.Unsubscribe(Messages.AuthLoadingFinish, LoadingOver);
        }

        private void LoadingStart()
        {
            IsLoading = true;
        }
        private void LoadingOver()
        {
            IsLoading = false;
        }
        protected override MainAuthModel GetModel()
        {
            return new MainAuthModel();
        }

        private async Task TryToLoadApi()
        {
            if (Preferences.Default.IsSaveAuth != false)
            {
                IApi? api = await Model.TryGetApi();
                ReturnApi.Invoke(api);
            }
        }

        private void OpenPreviousPage()
        {
            BackNavigationVisibility = Visibility.Collapsed;
            SwitchPageVisibility = Visibility.Visible;
            if (PreviousPage != null)
                EnteringFrame = PreviousPage;
        }
        private void OpenSettings()
        {
            BackNavigationVisibility=Visibility.Visible;
            SwitchPageVisibility = Visibility.Collapsed;
            PreviousPage = EnteringFrame;
            EnteringFrame = Settings;
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

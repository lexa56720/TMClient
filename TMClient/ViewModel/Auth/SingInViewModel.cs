using ApiWrapper.Interfaces;
using AsyncAwaitBestPractices.MVVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model.Auth;
using TMClient.Utils;

namespace TMClient.ViewModel.Auth
{
    class SingInViewModel(Func<IApi?, bool> returnApi) : BaseAuthViewModel(returnApi)
    {
        public ICommand SignInCommand => new AsyncCommand<PasswordBox>(SignIn, (o) => IsNotBusy);
        public ICommand OpenSettings => new AsyncCommand(() => Messenger.Send(Messages.OpenSettingsPage));
        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (value)
                    Messenger.Send(Messages.AuthLoadingFinish);
                else
                    Messenger.Send(Messages.AuthLoadingStart);
                isNotBusy = value;
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
        private bool isNotBusy = true;
        public string Login { get; set; } = string.Empty;

        public Visibility ErrorVisibility
        {
            get => errorVisibility;
            set
            {
                errorVisibility = value;
                OnPropertyChanged(nameof(ErrorVisibility));
            }
        }
        private Visibility errorVisibility = Visibility.Collapsed;

        private async Task SignIn(PasswordBox? passwordBox)
        {
            IsNotBusy = false;

            var password = passwordBox.Password;
            IApi? api = await SignInModel.SignIn(Login, password);


            if (!ReturnApi(api))
            {
                ErrorVisibility = Visibility.Visible;
                IsNotBusy = true;
            }
    
            passwordBox.Password = string.Empty;
        }
    }
}

using ApiWrapper.Interfaces;
using AsyncAwaitBestPractices.MVVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model.Auth;
using TMClient.Utils;

namespace TMClient.ViewModel.Auth
{
    class SignUpViewModel(Func<IApi?, bool> returnApi) : BaseAuthViewModel(returnApi)
    {
        public string UserName { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

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
        public ICommand SignUpCommand => new AsyncCommand<PasswordBox>(SignUp, o => IsNotBusy);

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

        private async Task SignUp(PasswordBox? passwordBox)
        {
            IsNotBusy = false;
            var password = passwordBox.Password;

            IApi? api = await SignUpModel.Registration(UserName, Login, password);

            if (!ReturnApi(api))
            {
                ErrorVisibility = Visibility.Visible;
                IsNotBusy = true;
            }
            passwordBox.Password = string.Empty;
        }
    }
}

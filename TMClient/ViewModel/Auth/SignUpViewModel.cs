using ApiWrapper.Interfaces;
using AsyncAwaitBestPractices.MVVM;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model.Auth;
using TMClient.Utils;

namespace TMClient.ViewModel.Auth
{
    class SignUpViewModel(Func<IApi?, bool> returnApi) : BaseAuthViewModel(returnApi),IDataErrorInfo
    {
        public string UserName { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));

                if (value)
                    Messenger.Send(Messages.AuthLoadingStart, true);
                else
                    Messenger.Send(Messages.AuthLoadingFinish, true);
            }
        }
        private bool isBusy = false;
        public ICommand SignUpCommand => new AsyncCommand<PasswordBox>(SignUp, o => !IsBusy);
        public ICommand OpenSettings => new AsyncCommand(() => Messenger.Send(Messages.OpenSettingsPage));

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


        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Login":
                        if (Login != string.Empty && !Model.IsLoginValid(Login))
                            return "Логин не соответствует требованиям";
                        break;
                    case "UserName":
                        if (UserName != string.Empty && !Model.IsNameValid(UserName))
                            return "Логин не соответствует требованиям";
                        break;
                }
                return string.Empty;
            }
        }

        private readonly SignUpModel Model = new();
        private async Task SignUp(PasswordBox? passwordBox)
        {    
            var password = passwordBox.Password;
            if (!Model.IsLoginValid(Login) || !Model.IsPasswordValid(password))
            {
                ErrorVisibility = Visibility.Visible;
                return;
            }

            IsBusy = true;
            IApi? api = await Model.Registration(UserName, Login, password);

            if (!ReturnApi(api))
            {
                ErrorVisibility = Visibility.Visible;
                IsBusy = false;
            }
            passwordBox.Password = string.Empty;
        }
    }
}

using ClientApiWrapper.Interfaces;
using AsyncAwaitBestPractices.MVVM;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model.Auth;
using TMClient.Utils;
using TMClient.Utils.Validations;

namespace TMClient.ViewModel.Auth
{
    class SignUpViewModel : BaseAuthViewModel<SignUpModel>, IDataErrorInfo
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

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(RepeatPassword));
                OnPropertyChanged(nameof(Password));
            }
        }
        private string password = string.Empty;

        public string RepeatPassword
        {
            get => repeatPassword;
            set
            {
                repeatPassword = value;
                OnPropertyChanged(nameof(RepeatPassword));
            }
        }
        private string repeatPassword = string.Empty;

        public string ErrorText
        {
            get => errorText;
            set
            {
                errorText = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }
        private string errorText;

        public ICommand SignUpCommand => new AsyncCommand(SignUp, o => !IsBusy);
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

        public string Error => throw new NotImplementedException();

        public string this[string columnName] => GetError(columnName);

        private Visibility errorVisibility = Visibility.Collapsed;

        public SignUpViewModel(Func<IApi?, bool> returnApi) : base(returnApi)
        {
        }
        protected override SignUpModel GetModel()
        {
            return new SignUpModel();
        }

        private string GetError(string columnName)
        {
            switch (columnName)
            {
                case nameof(Password):
                    {
                        var a = new PasswordRule();
                        var b = a.Validate(Password, null);
                        if (!b.IsValid && !string.IsNullOrEmpty(Password))
                            return b.ErrorContent.ToString();
                        break;
                    }
                case nameof(RepeatPassword):
                    {
                        if (!Password.Equals(RepeatPassword))
                            return "Пароли не совпадают";
                        break;
                    }
            }
            return string.Empty;
        }

        private async Task SignUp()
        {
            if (!Model.IsLoginValid(Login) || !Model.IsPasswordValid(Password))
            {
                ErrorVisibility = Visibility.Visible;
                return;
            }
            IsBusy = true;
            try
            {
                IApi? api = null;
                await Task.Run(async () =>
                  {
                      api = await Model.Registration(UserName, Login, Password);
                  });

                if (!ReturnApi(api))
                {
                    ErrorVisibility = Visibility.Visible;
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
                ErrorVisibility = Visibility.Visible;
                IsBusy = false;
            }

            Password = string.Empty;
            RepeatPassword = string.Empty;
        }
    }
}

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
    class SingInViewModel : BaseAuthViewModel<SignInModel>
    {
        public ICommand SignInCommand => new AsyncCommand(SignIn, (o) => !IsBusy);
        public ICommand OpenSettings => new AsyncCommand(() => Messenger.Send(Messages.OpenSettingsPage));
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

        public string Login { get; set; } = string.Empty;

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private string password = string.Empty;

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


        public SingInViewModel(Func<IApi?, bool> returnApi) : base(returnApi)
        {
        }
        protected override SignInModel GetModel()
        {
            return new SignInModel();
        }
        private async Task SignIn()
        {      
            if (!Model.IsLoginValid(Login))
            {
                ErrorVisibility = Visibility.Visible;
                return;
            }

            IsBusy = true;
            try
            {
                IApi? api = await Model.SignIn(Login, Password);
                if (!ReturnApi(api))
                {
                    ErrorVisibility = Visibility.Visible;
                    IsBusy = false;
                }
            }
            catch(Exception ex)
            {
                ErrorText = ex.Message;
                ErrorVisibility = Visibility.Visible;
                IsBusy = false;
            }


            Password = string.Empty;
        }


    }
}

using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model.Auth;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Auth
{
    class AuthViewModel : BaseAuthViewModel
    {

        public ICommand SignInCommand => new AsyncCommand<PasswordBox>(SignIn, (o) => IsNotBusy);

        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                isNotBusy = value;
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
        private bool isNotBusy=true;
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
            var api = await SignInModel.SignIn(Login, password);
            passwordBox.Password = string.Empty;
            if (api != null)
               await OpenMainWindow(api);

            ErrorVisibility = Visibility.Visible;
            IsNotBusy = true;
        }

    }
}

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
    class SignUpViewModel : BaseAuthViewModel
    {

        public string UserName { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;
        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                isNotBusy = value;
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
        private bool isNotBusy = true;
        public ICommand SignUpCommand => new AsyncCommand<PasswordBox>(SignUp,o=>IsNotBusy);

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
          
            var api = await SignUpModel.Registration(UserName, Login, password);
            passwordBox.Password = string.Empty;
            if (api != null)
                await OpenMainWindow(api);

            ErrorVisibility = Visibility.Visible;
            IsNotBusy = true;

        }
    }
}

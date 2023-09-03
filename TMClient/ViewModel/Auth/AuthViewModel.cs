﻿using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Auth
{
    class AuthViewModel : BaseViewModel
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
            passwordBox.Password = string.Empty;
            var api = await SignInModel.SignIn(Login, password);
            if (api != null)
            {
                var mainWindow = new MainWindow(api);
                mainWindow.Show();
                await Messenger.Send(Messages.CloseAuth);
                return;
            }
            ErrorVisibility = Visibility.Visible;
            IsNotBusy = true;
        }

    }
}

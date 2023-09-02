﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Utils;

namespace TMClient.ViewModel.Auth
{
    class AuthViewModel : BaseViewModel
    {

        public ICommand SignInCommand => new Command(o => SignIn((PasswordBox)o));

        public string Login { get; set; }

        private void SignIn(PasswordBox passwordBox)
        {
            Console.WriteLine(passwordBox.Password);
            passwordBox.Password = string.Empty;
        }

    }
}
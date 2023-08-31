using System;
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

        public ICommand Login => new Command(o => LoginIn((PasswordBox)o));

        public string Username { get; set; }

        private void LoginIn(PasswordBox passwordBox)
        {
            Console.WriteLine(passwordBox.Password);
            passwordBox.Password = string.Empty;
        }

    }
}

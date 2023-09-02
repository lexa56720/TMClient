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
    class RegisterViewModel
    {
        public string UserName { get; set; }

        public string Login { get; set; }

        public ICommand SignUpCommand => new Command(o=> SignUp((PasswordBox)o));

        private void SignUp(PasswordBox passwordBox)
        {
            Console.WriteLine(passwordBox.Password);
            passwordBox.Password = string.Empty;
        }
    }
}

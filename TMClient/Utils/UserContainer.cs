using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TMClient.Utils
{
    internal class UserContainer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public bool IsRequested
        {
            get => isRequested;
            set
            {
                isRequested = value;
                OnPropertyChanged(nameof(IsRequested));
            }
        }
        private bool isRequested;
        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }
        private Visibility visibility;
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        private User user = null!;
        public UserContainer(User user,Visibility visibility=Visibility.Visible)
        {
            User = user;
            Visibility = visibility;
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

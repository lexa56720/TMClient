using ApiWrapper.Interfaces;
using System.ComponentModel;

namespace TMClient.ViewModel
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public static IUserInfo CurrentUser { protected get; set; } = null!;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TMClient.Utils;

namespace TMClient.ViewModel
{
    internal class SettingsViewModel : BaseViewModel
    {
        public Visibility BackNavigationVisibility => BackNavigation == null ? Visibility.Collapsed : Visibility.Visible;

        public ICommand? BackNavigation { get; set; }
        public SettingsViewModel(Action openPreviousPage)
        {
            BackNavigation = new Command(openPreviousPage);
        }
        public SettingsViewModel()
        {
        }
    }
}

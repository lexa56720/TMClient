using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Auth
{
    class MainAuthViewModel : BaseViewModel
    {
        public Page EnteringFrame
        {
            get => enteringFrame;
            set
            {
                enteringFrame = value;
                OnPropertyChanged(nameof(EnteringFrame));
            }
        }
        private Page enteringFrame;

        private Page Registration { get; } = new RegisterView();

        private Page Auth { get; } = new AuthView();

        public ICommand Switch => new Command(SwitchPages);

        public MainAuthViewModel()
        {
           // EnteringFrame = Auth;
        }

        private void SwitchPages()
        {
            if (EnteringFrame == Registration)
                EnteringFrame = Auth;
            else
                EnteringFrame = Registration;
        }
    }
}

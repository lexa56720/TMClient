using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    class MainAuthViewModel : BaseAuthViewModel
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
        private Page enteringFrame = null!;

        private Page Registration { get; } = new SignUpView();

        private Page Auth { get; } = new AuthView();

        public ICommand Switch => new Command(SwitchPages);

        public bool IsLoaded
        {
            get => isLoaded;
            set
            {
                isLoaded = value;
                if (value)
                    LoadingVisibility = Visibility.Hidden; 
                else
                    LoadingVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }
        private bool isLoaded=false;

        public Visibility LoadingVisibility
        {
            get => loadingVisibility;
            set
            {
                loadingVisibility = value;
                OnPropertyChanged(nameof(LoadingVisibility));
            }
        }
        private Visibility loadingVisibility = Visibility.Visible;

        public MainAuthViewModel()
        {
            EnteringFrame = Auth;
            TryToLoadAuth();
        }

        private void TryToLoadAuth()
        {
            Task.Run(async () =>
            {
                IsLoaded = false;
                var api = await AuthModel.TryGetApi();
                if (api != null)
                {
                    await OpenMainWindow(api);
                    return;
                }

                IsLoaded = true;        
            });
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

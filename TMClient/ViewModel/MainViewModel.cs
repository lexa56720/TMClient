using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Types;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public Page SidePanelFrame
        {
            get => sidePanelFrame;
            set
            {
                sidePanelFrame = value;
                OnPropertyChanged(nameof(SidePanelFrame));
            }
        }
        private Page sidePanelFrame;

        public ICommand ChangeSideBarState => new Command(SwitchSideBarState);


        public Visibility SideBarState
        {
            get => sideBarState;
            set
            {
                sideBarState = value;
                OnPropertyChanged(nameof(SideBarState));
            }
        }
        private Visibility sideBarState = Visibility.Collapsed;


        public Page MainFrame
        {
            get => mainFrame;
            set
            {
                mainFrame = value;
                OnPropertyChanged(nameof(MainFrame));
            }
        }
        private Page mainFrame;


        private SidePanel Panel;

        public MainViewModel()
        {
            Panel = new SidePanel();
            MainFrame = new ChatView(new Chat() { Id=0, Name="намба ван чат"});
            SidePanelFrame = Panel;
        }

        public void SwitchSideBarState()
        {
            if(SideBarState== Visibility.Collapsed)
                SideBarState = Visibility.Visible;
            else
                SideBarState= Visibility.Collapsed;
        }
    }
}

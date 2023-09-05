using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TMClient.View;

namespace TMClient.ViewModel
{
    class MainViewModel:BaseViewModel
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
            SidePanelFrame = Panel;
        }
    }
}

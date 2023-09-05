using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Types;

namespace TMClient.ViewModel
{
    class SidePanelViewModel : BaseViewModel
    {
        public ObservableCollection<Friend> Friends
        {
            get => friends;
            set
            {
                friends = value;
                OnPropertyChanged(nameof(Friends));
            }
        }

        private ObservableCollection<Friend> friends = new ObservableCollection<Friend>();

        public SidePanelViewModel()
        {
            for (int i = 0; i < 20; i++)
                Friends.Add(new Friend() { Name = "Ffff" });
        }

    }
}

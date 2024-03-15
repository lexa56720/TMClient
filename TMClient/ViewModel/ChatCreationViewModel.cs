using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Model;

namespace TMClient.ViewModel
{
    internal class ChatCreationViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string name = string.Empty;


        private readonly ChatCreationModel Model = new();
        private readonly Action<Chat?> dialogCompleted;

        public ICommand CreateCommand => new AsyncCommand(Confirm);

        public ChatCreationViewModel(User[] users, Action<Chat?> dialogCompleted)
        {
            Users = new ObservableCollection<User>(users);
            this.dialogCompleted = dialogCompleted;
        }

        private async Task Confirm()
        {
            var chat = await Model.CreateChat(Users, Name);
            dialogCompleted(chat);
        }
    }
}

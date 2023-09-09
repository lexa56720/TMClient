using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Controls;
using TMClient.Model.Chats;
using TMClient.Types;

namespace TMClient.ViewModel.Chats
{
    internal abstract class BaseChatViewModel<T> : BaseViewModel where T : BaseChatModel
    {
        protected Chat Chat { get; }
        public ObservableCollection<MessageControl> Messages { get; set; } = new();

        public string ChatName
        {
            get => chatName;
            set
            {
                chatName = value;
                OnPropertyChanged(nameof(ChatName));
            }
        }
        private string chatName = string.Empty;

        protected T Model { get; private set; }

        public BaseChatViewModel(Chat chat)
        {
            ChatName = chat.Name;
            Model = GetModel(chat);
            Chat = chat;
        }

        protected abstract T GetModel(Chat chat);


        protected void AddMessage(Message message)
        {

        }
    }
}

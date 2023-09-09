using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Model.Chats;
using TMClient.Types;

namespace TMClient.ViewModel.Chats
{
    internal class ChatViewModel : BaseChatViewModel<ChatModel>
    {
        public int MemberCount
        {
            get => memberCount;
            set
            {
                memberCount = value;
                OnPropertyChanged(nameof(MemberCount));
            }
        }
        private int memberCount;


        public ICommand LeaveCommand => new AsyncCommand(LeaveChat);
        public ICommand InviteCommand => new AsyncCommand(InviteToChat);
        public ICommand ShowMembersCommand => new AsyncCommand(ShowMembers);


        public ChatViewModel(Chat chat) : base(chat)
        {
        }

        protected override ChatModel GetModel(Chat chat)
        {
            return new ChatModel(chat);
        }

        private async Task LeaveChat()
        {

        }
        private async Task InviteToChat()
        {

        }
        private async Task ShowMembers()
        {
            throw new NotImplementedException();
        }
    }
}

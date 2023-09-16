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
            Task.Run(async () =>
            {
                int count = 0;
                while (true)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    AddMessageToStart(new Message()
                    {
                        Text = "Nice man "+ count++,
                        Destionation = chat,
                        Id = 1,
                        Author = new User()
                        {
                            Id = Random.Shared.NextDouble() > 0.5f ? App.Api.Id : 1,
                            IsOnline = true,
                            Name = "samuel"
                        }

                    }));
                    await Task.Delay(300);
                }

            });
        }

        protected override ChatModel GetModel(Chat chat)
        {
            return new ChatModel(chat);
        }

        private async Task LeaveChat()
        {
            throw new NotImplementedException();
        }
        private async Task InviteToChat()
        {
            throw new NotImplementedException();
        }
        private async Task ShowMembers()
        {
            throw new NotImplementedException();
        }
    }
}

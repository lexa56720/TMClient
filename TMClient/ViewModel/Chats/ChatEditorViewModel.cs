using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TMClient.Model.Chats;
using TMClient.Utils;

namespace TMClient.ViewModel.Chats
{
    internal class ChatEditorViewModel : BaseViewModel
    {
        public ICommand OpenChatCommand => new Command(OpenChatPage);
        public ICommand SaveCommand => new AsyncCommand(Save);
        public ICommand PickImageCommand => new AsyncCommand(PickImage);
        public ICommand KickUserCommand => new AsyncCommand<ChatMember>(KickUser);

        public Chat Chat
        {
            get => chat;
            set
            {
                chat = value;
                OnPropertyChanged(nameof(Chat));
            }
        }
        private Chat chat = null!;

        public string ChatName
        {
            get => chatName;
            set
            {
                chatName = value;
                OnPropertyChanged(nameof(ChatName));
            }
        }
        private string chatName=string.Empty;

        public ObservableCollection<ChatMember> Members { get; set; } = new();

        private readonly Action OpenChatPage;

        private readonly ChatEditorModel Model=new();
        public ChatEditorViewModel(Chat chat, Action openChatPage)
        {
            Chat = chat;
            ChatName = chat.Name;
            OpenChatPage = openChatPage;
            foreach (var user in chat.Members)
            {
                if (user.IsCurrentUser)
                    continue;

                var isAdmin = user.Id == chat.Admin.Id;
                var member = new ChatMember(user, Visibility.Visible, isAdmin);
                if (isAdmin)
                    Members.Insert(0, member);
                else
                    Members.Add(member);
            }
        }

        private async Task PickImage()
        {
            var imageData = PathPicker.GetImageData();
            if (imageData.Length > 0)
                await Model.ChangeCover(Chat, imageData);
        }

        private async Task Save()
        {
            if (!Chat.Name.Equals(ChatName))
                await Model.ChangeName(Chat,ChatName);
        }

        private async Task KickUser(ChatMember? member)
        {
            if (member == null)
                return;
            await Model.KickUser(member.User, Chat);
            Members.Remove(member);
        }
    }
}

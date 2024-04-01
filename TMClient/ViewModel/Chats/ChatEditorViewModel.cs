using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string chatName;


        private readonly Action OpenChatPage;

        private readonly ChatEditorModel Model=new();
        public ChatEditorViewModel(Chat chat, Action openChatPage)
        {
            Chat = chat;
            ChatName = chat.Name;
            OpenChatPage = openChatPage;
        }

        private async Task PickImage()
        {
            var imageData = FileImageData.GetImageData();
            if (imageData.Length > 0)
                await Model.ChangeCover(Chat, imageData);
        }

        private async Task Save()
        {
            if (!Chat.Name.Equals(ChatName))
                await Model.ChangeName(Chat,ChatName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Utils;
using TMClient.View.Chats;

namespace TMClient.ViewModel.Chats
{
    internal class ChatViewModel : BaseViewModel
    {
        public Chat Chat { get; }
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        private Page currentPage = null!;
        public Page ChatPage
        {
            get => chatPage;
            set
            {
                chatPage = value;
                OnPropertyChanged(nameof(ChatPage));
            }
        }
        private Page chatPage;
        public Page? ChatEditor
        {
            get => chatEditor;
            set
            {
                chatEditor = value;
                OnPropertyChanged(nameof(ChatEditor));
            }
        }
        private Page? chatEditor;

        public ChatViewModel(Chat chat)
        {
            Chat = chat;
            ChatPage = new MultiUserChat(Chat, OpenEditorPage);
            CurrentPage = ChatPage;
        }

        private void OpenEditorPage()
        {
            ChatEditor ??= new ChatEditor(Chat,OpenChatPage);
            CurrentPage = ChatEditor;
        }

        private void OpenChatPage()
        {
            CurrentPage = ChatPage;
        }
    }
}

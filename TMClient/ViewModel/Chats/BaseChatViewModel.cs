using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using TMClient.Controls;
using TMClient.Model.Chats;
using TMClient.Types;

namespace TMClient.ViewModel.Chats
{
    internal abstract class BaseChatViewModel<T> : BaseViewModel where T : BaseChatModel
    {
        public int Id { get; private set; }
        protected Chat Chat { get; }
        public ObservableCollection<MessageControl> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }
        private ObservableCollection<MessageControl> messages = new();
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

        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }
        private string messageText = string.Empty;


        public ICommand LoadHistory => new AsyncCommand(LoadMessages);
        public ICommand Send => new AsyncCommand<string>(SendMessage);
        public ICommand Attach => new AsyncCommand(AttachFile);

        protected T Model { get; private set; }

        public BaseChatViewModel(Chat chat)
        {
            ChatName = chat.Name;
            Chat = chat;
            Id = chat.Id;

            Model = GetModel(chat);

        }


        protected abstract T GetModel(Chat chat);

        public async Task LoadMessages()
        {
            if (!Messages.Any())
            {
                AddMessageToStart(await Model.GetHistory(0));
                return;
            }


            var messages = await Model.GetHistory(Messages.First().InnerMessages.First());
            AddMessageToStart(messages);
        }

        public async Task SendMessage(string? text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var message = await Model.SendMessage(new Message()
            {
                Author = App.CurrentUser,
                Text = text,
                Destionation = Chat,
            });
            if (message != null)
                AddMessageToEnd(message);
            MessageText = string.Empty;
        }

        public async Task AttachFile()
        {

        }

        protected void AddMessageToEnd(Message message)
        {
            var last = Messages.LastOrDefault();
            if (last != null && IsUnionable(last, message))
                last.UnionToEnd(message);
            else
                Messages.Add(new MessageControl(message));
        }
        protected void AddMessageToEnd(Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                AddMessageToEnd(messages[i]);
        }

        protected void AddMessageToStart(Message message)
        {
            var first = Messages.FirstOrDefault();
            if (first != null && IsUnionable(first, message))
                first.UnionToStart(message);
            else
                Messages.Insert(0, new MessageControl(message));
        }
        protected void AddMessageToStart(Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                AddMessageToStart(messages[i]);
        }

        private bool IsUnionable(MessageControl oldMessage, Message newMessage)
        {
            return oldMessage.Author.Id == newMessage.Author.Id &&
                oldMessage.InnerMessages.First().SendTime - newMessage.SendTime < TimeSpan.FromMinutes(5) &&
                oldMessage.InnerMessages.Count < 10;
        }
    }
}

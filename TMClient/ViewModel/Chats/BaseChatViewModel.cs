using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Controls;
using TMClient.Model.Chats;
using TMClient.Types;

namespace TMClient.ViewModel.Chats
{
    internal abstract class BaseChatViewModel<T> : BaseViewModel where T : BaseChatModel
    {
        public int Id { get; private set; }
        protected Chat Chat { get; }
        public ObservableCollection<MessageControl> Messages { get; set; } = new();

        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }
        private string messageText;

        public ICommand LoadHistory => new AsyncCommand(LoadMessages);

        public ICommand Send => new AsyncCommand<string>(SendMessage);
        public ICommand Attach => new AsyncCommand(AttachFile);


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
            Chat = chat;
            Id = chat.Id;

            Model = GetModel(chat);
        }

        protected abstract T GetModel(Chat chat);

        public async Task LoadMessages()
        {
            if (Messages.Any())
                await Model.GetHistory(Messages.Last().InnerMessages.Last());
        }

        public async Task SendMessage(string text)
        {
            var message = await Model.SendMessage(new Message()
            {
                Author = App.CurrentUser,
                Text = text,
                Destionation = Chat,
            });
            if (message != null)
                Messages.Add(new MessageControl(message));
            MessageText = string.Empty;
        }

        public async Task AttachFile()
        {

        }



        protected void AddMessage(Message message)
        {
            var last = Messages.LastOrDefault();
            if (last != null && last.Author.Id == message.Author.Id)
                last.UnionToEnd(message);
            else
                Messages.Add(new MessageControl(message));
        }
        protected void AddMessage(Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                AddMessage(messages[i]);
        }

        protected void AddMessageToStart(Message message)
        {
            var first = Messages.FirstOrDefault();
            if (first != null && first.Author.Id == message.Author.Id)
                first.UnionToStart(message);
            else
                Messages.Insert(0, new MessageControl(message));
        }
        protected void AddMessageToStart(Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                AddMessageToStart(messages[i]);
        }
    }
}

using AsyncAwaitBestPractices.MVVM;
using ClientApiWrapper.Types;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TMClient.Controls;
using TMClient.Model.Chats;
using TMClient.Utils;

namespace TMClient.ViewModel.Chats
{
    internal abstract class BaseChatViewModel<T> : BaseViewModel where T : ChatModel
    {
        public int Id { get; private set; }
        public Chat Chat { get; private set; }
        public ObservableCollection<MessageBaseControl> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }
        private ObservableCollection<MessageBaseControl> messages = new();
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

        public ICommand PageLoadedCommand => new Command(PageLoaded);
        public ICommand PageUnloadedCommand => new Command(PageUnloaded);

        protected T Model { get; private set; }

        private bool IsFullyLoaded = false;
        public BaseChatViewModel(Chat chat)
        {
            chat.UnreadCount = 0;
            Chat = chat;
            Id = chat.Id;

            Model = GetModel(chat);
        }

        protected abstract T GetModel(Chat chat);
        private void PageLoaded(object? obj)
        {
            CurrentUser.NewMessages += HandleNewMessages;
            CurrentUser.ReadedMessages += ReadMessages;
        }
        private void PageUnloaded(object? obj)
        {
            CurrentUser.NewMessages -= HandleNewMessages;
            CurrentUser.ReadedMessages -= ReadMessages;
        }

        public async Task LoadMessages()
        {
            if (IsFullyLoaded)
                return;

            Message[] messages;
            if (Messages.Any())
                messages = await Model.GetHistory(Messages.First().Message);
            else
                messages = await Model.GetHistory(0);

            if (messages.Length < Model.Count)
                IsFullyLoaded = true;

            var readedMessages = messages.Where(m => !m.IsReaded && !m.IsOwn)
                                         .ToArray();
            await Model.MarkAsReaded(readedMessages);

            AddMessageToStart(messages);
        }

        public async Task SendMessage(string? text)
        {
            MessageText = string.Empty;

            var message = await Model.SendMessage(text, CurrentUser.Info);

            if (message != null)
                AddMessageToEnd(message);
            Model.SetIsReaded(Messages.Where(m => !m.Message.IsOwn && !m.Message.IsReaded));
        }

        public async Task AttachFile()
        {

        }

        protected async void HandleNewMessages(object? sender, Message[] messages)
        {
            var currentChatMessages = messages.Where(m => m.Destination.Id == Chat.Id)
                                              .ToArray();
            await Model.MarkAsReaded(currentChatMessages);

            App.MainThread.Invoke(() => AddMessageToEnd(currentChatMessages));
        }
        private void ReadMessages(object? sender, int[] e)
        {
            Model.SetIsReaded(Messages.Where(m => e.Contains(m.Message.Id)));
        }

        private MessageBaseControl CreateMessage(Message message)
        {
            if (message.IsSystem)
                return new SystemMessageControl((SystemMessage)message);
            return new MessageControl(message);
        }
        protected void AddMessageToEnd(params Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                Messages.Add(CreateMessage(messages[i]));
        }
        protected void AddMessageToStart(params Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                Messages.Insert(0, CreateMessage(messages[i]));
        }
    }
}

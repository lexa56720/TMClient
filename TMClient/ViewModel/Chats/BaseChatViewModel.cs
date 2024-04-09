using AsyncAwaitBestPractices.MVVM;
using ClientApiWrapper.Types;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using TMClient.Controls;
using TMClient.Model.Chats;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Chats
{
    internal abstract class BaseChatViewModel<T> : BaseViewModel where T : ChatModel
    {
        public int Id { get; private set; }
        public Chat Chat { get; private set; }
        public ObservableCollection<MessageContainer> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }
        private ObservableCollection<MessageContainer> messages = [];
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
        public ICommand PageLoadedCommand => new Command(PageLoaded);
        public ICommand PageUnloadedCommand => new Command(PageUnloaded);
        public ICommand OpenImageCommand => new Command(OpenImage);

        private void OpenImage(object? obj)
        {
            if (obj is not ImageSource image)
                return;
            var imageViewer = new ImageViewerWindow(image);
            imageViewer.Show();
        }

        public ObservableCollection<string> Files { get; set; } = [];

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

            Message? sendedMessage;
            if (Files.Count == 0)
                sendedMessage = await Model.SendMessage(text);
            else
                sendedMessage = await Model.SendMessage(text, Files.ToArray());

            if (sendedMessage != null)
            {
                AddMessageToEnd(sendedMessage);

                MessageText = string.Empty;
                Files.Clear();
            }
            Model.SetIsReaded(Messages.Where(m => !m.Message.IsOwn && !m.Message.IsReaded)
                                      .Select(m => m.Message));
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
            Model.SetIsReaded(Messages.Where(m => e.Contains(m.Message.Id))
                                      .Select(m => m.Message));
        }

        private MessageContainer CreateMessage(Message message)
        {
            return new MessageContainer(message);
        }
        protected void AddMessageToEnd(params Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                var message = CreateMessage(messages[i]);
                if (!message.Message.IsSystem && Messages.LastOrDefault() is MessageContainer prev &&
                    !prev.Message.IsSystem && prev.Message.Author.Id != message.Message.Author.Id)
                {
                    message.IsAuthorVisible = false;
                }
                Messages.Add(message);
            }

        }
        protected void AddMessageToStart(params Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                var message = CreateMessage(messages[i]);
                if (!message.Message.IsSystem)
                {
                    if (Messages.FirstOrDefault() is MessageContainer next && !next.Message.IsSystem)
                    {
                        if (next.Message.Author.Id == messages[i].Author.Id)
                            next.IsAuthorVisible = false;
                        message.IsAuthorVisible = true;
                    }
                    else if (Messages.FirstOrDefault() is MessageContainer nextM && nextM.Message.IsSystem)
                    {
                        message.IsAuthorVisible = true;
                    }
                }

                Messages.Insert(0, message);
            }
        }
    }
}

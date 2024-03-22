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
                messages = await Model.GetHistory(Messages.First().InnerMessages.First());
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

            if (string.IsNullOrEmpty(text))
                return;

            var message = await Model.SendMessage(new Message()
            {
                Author = CurrentUser.Info,
                Text = text,
                Destination = Chat,
                IsReaded = false,
                IsOwn = true
            });

            if (message != null)
                AddMessageToEnd(message);


            ReadMessages(null, Messages.SelectMany(m => m.InnerMessages)
                                      .Where(m => !m.IsReaded && !m.IsOwn)
                                      .Select(m => m.Id)
                                      .ToArray());
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
            //if(e.Length == 0) return;
            var affectedMessages = new List<MessageBaseControl>();

            for (int i = 0; i < Messages.Count; i++)
            {
                if (Messages[i].InnerMessages.Any(im => e.Contains(im.Id)))
                    affectedMessages.Add(Messages[i]);
            }

            for (int i = 0; i < affectedMessages.Count; i++)
            {
                foreach (var innerMessage in affectedMessages[i].InnerMessages)
                    if (e.Contains(innerMessage.Id))
                        innerMessage.IsReaded = true;
            }
            App.MainThread.Invoke(() =>
            {
                for (int i = 0; i < affectedMessages.Count; i++)
                {
                    if (affectedMessages[i].InnerMessages.All(m => m.IsReaded))
                    {
                        affectedMessages[i].IsReaded = true;
                        continue;
                    }

                    if (affectedMessages[i] is MessageControl)
                    {
                        var splitted = Split((MessageControl)affectedMessages[i]);
                        var index = Messages.IndexOf(affectedMessages[i]);

                        Messages.RemoveAt(index);
                        for (int j = 0; j < splitted.Count; j++)
                            Messages.Insert(index + j, splitted[j]);
                    }
                }

                for (int i = 0; i < (Messages.Count - 1); i++)
                {
                    if (!Messages[i].IsCanUnion(Messages[i + 1]))
                        continue;

                    for (int j = 0; j < Messages[i + 1].InnerMessages.Count; j++)
                        Messages[i].UnionToEnd(Messages[i + 1].InnerMessages.ElementAt(j));

                    Messages.RemoveAt(i + 1);
                    i--;
                }
            });
        }


        private MessageBaseControl CreateMessage(Message message)
        {
            if (message.IsSystem)
                return new SystemMessageControl((SystemMessage)message);
            return new MessageControl(message);
        }

        protected void AddMessageToEnd(Message message)
        {
            var last = Messages.LastOrDefault();
            if (last != null && last.IsCanUnion(message))
                last.UnionToEnd(message);
            else
                Messages.Add(CreateMessage(message));
        }
        protected void AddMessageToEnd(Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                AddMessageToEnd(messages[i]);
        }

        protected void AddMessageToStart(Message message)
        {
            var first = Messages.FirstOrDefault();
            if (first != null && first.IsCanUnion(message))
                first.UnionToStart(message);
            else
                Messages.Insert(0, CreateMessage(message));
        }
        protected void AddMessageToStart(Message[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
                AddMessageToStart(messages[i]);
        }

        protected List<MessageControl> Split(MessageControl message)
        {
            var innerMessages = message.InnerMessages;
            var result = new List<MessageControl>(innerMessages.Count)
            {
                new(innerMessages.First())
            };
            for (int i = 1; i < innerMessages.Count; i++)
            {
                if (result.Last().IsCanUnion(innerMessages.ElementAt(i)))
                {
                    result.Last().UnionToEnd(innerMessages.ElementAt(i));
                }
                else
                {
                    result.Add(new MessageControl(innerMessages.First()));
                }
            }
            return result;
        }

    }
}

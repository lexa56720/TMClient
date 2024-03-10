using AsyncAwaitBestPractices.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TMClient.Controls;
using TMClient.Model.Chats;
using TMClient.Utils;

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
            CurrentUser.NewMessages += UpdateMessages;
            CurrentUser.ReadedMessages += ReadedMessages;
        }



        protected abstract T GetModel(Chat chat);

        public async Task LoadMessages()
        {
            Message[] messages;
            if (Messages.Any())
                messages = await Model.GetHistory(Messages.First().InnerMessages.First());
            else
                messages = await Model.GetHistory(0);


            var readedMessages = messages.Where(m => !m.IsReaded && m.Author.Id != CurrentUser.Info.Id)
                                         .ToArray();
            await Model.MarkAsReaded(readedMessages);

            AddMessageToStart(messages);
        }

        public async Task SendMessage(string? text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var message = await Model.SendMessage(new Message()
            {
                Author = CurrentUser.Info,
                Text = text,
                Destionation = Chat,
                IsReaded = false,
            });
            if (message != null)
                AddMessageToEnd(message);
            MessageText = string.Empty;
        }

        public async Task AttachFile()
        {

        }


        protected async void UpdateMessages(object? sender, Message[] messages)
        {
            var currentChatMessages = messages.Where(m => m.Destionation.Id == Chat.Id)
                                              .ToArray();
            await Model.MarkAsReaded(currentChatMessages);

            App.MainThread.Invoke(() => AddMessageToEnd(currentChatMessages));  
        }
        private void ReadedMessages(object? sender, int[] e)
        {
            var affectedMessages = new List<MessageControl>();

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
                        affectedMessages[i].IsReaded = true;
                    else
                    {
                        var splitted = Split(affectedMessages[i]);
                        var index = Messages.IndexOf(affectedMessages[i]);

                        Messages.RemoveAt(index);
                        for (int j = 0; j < splitted.Count; j++)
                            Messages.Insert(index + j, splitted[j]);
                    }
                }
            });
        }
        protected void AddMessageToEnd(Message message)
        {
            var last = Messages.LastOrDefault();
            if (last != null && IsUnionable(last, message))
                last.UnionToEnd(message);
            else
                Messages.Add(new MessageControl(message, CurrentUser));
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
                Messages.Insert(0, new MessageControl(message, CurrentUser));
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
                new(innerMessages.First(), CurrentUser)
            };
            for (int i = 1; i < innerMessages.Count; i++)
            {
                if (IsUnionable(result.Last(), innerMessages.ElementAt(i)))
                {
                    result.Last().UnionToEnd(innerMessages.ElementAt(i));
                }
                else
                {
                    result.Add(new MessageControl(innerMessages.First(), CurrentUser));
                }
            }
            return result;
        }

        private bool IsUnionable(MessageControl oldMessage, Message newMessage)
        {
            return oldMessage.Author.Id == newMessage.Author.Id &&
                   oldMessage.IsReaded == newMessage.IsReaded &&
                   oldMessage.InnerMessages.First().SendTime - newMessage.SendTime < TimeSpan.FromMinutes(5) &&
                   oldMessage.InnerMessages.Count < 10;
        }
    }
}

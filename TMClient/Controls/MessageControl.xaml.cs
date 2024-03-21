using ApiWrapper.Interfaces;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using TMClient.Model;
using TMClient.Utils;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для MessageControl.xaml
    /// </summary>
    public partial class MessageControl : MessageBaseControl, INotifyPropertyChanged
    {
        public override required User Author { get; init; }
        public override required bool IsOwn
        {
            get => isOwn;
            set
            {
                isOwn = value;
                OnPropertyChanged(nameof(IsOwn));
            }
        }
        private bool isOwn;
        public override required string Text
        {
            get
            {
                return string.Join(Environment.NewLine, Messages.Select(m => m.Text));
            }
            set
            {
                OnPropertyChanged(nameof(Text));
            }
        }

        public override required bool IsReaded
        {
            get => isReaded;
            set
            {
                isReaded = value;
                OnPropertyChanged(nameof(IsReaded));
            }
        }
        private bool isReaded;

        public override required DateTime Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
        private DateTime time;

        private List<Message> Messages { get; init; } = new();
        public override IReadOnlyCollection<Message> InnerMessages => Messages;


        [SetsRequiredMembers]
        public MessageControl(Message message)
        {
            Messages.Add(message);
            Author = message.Author;
            IsOwn = message.IsOwn;
            Text = message.Text;
            Time = message.SendTime;
            IsReaded = message.IsReaded;

            InitializeComponent();
            DataContext = this;
        }
        public MessageControl()
        {
            InitializeComponent();
            DataContext = this;
        }


        public override bool IsCanUnion(Message newMessage)
        {
            return !newMessage.IsSystem &&
                   Author.Id == newMessage.Author.Id &&
                   IsReaded == newMessage.IsReaded &&
                  (InnerMessages.First().SendTime - newMessage.SendTime).Duration() < TimeSpan.FromMinutes(5) &&
                   InnerMessages.Count < 10;
        }
        public override bool IsCanUnion(MessageBaseControl second)
        {
            return second is not SystemMessageControl &&
                   Author.Id == second.Author.Id &&
                   IsReaded == second.IsReaded &&
                  (InnerMessages.First().SendTime - second.InnerMessages.Last().SendTime).Duration() < TimeSpan.FromMinutes(5) &&
                   InnerMessages.Count + second.InnerMessages.Count < 10;
        }

        public override void UnionToEnd(Message message)
        {
            Messages.Add(message);
            OnPropertyChanged(nameof(Text));
        }

        public override void UnionToStart(Message message)
        {
            Messages.Insert(0, message);
            Time = message.SendTime;
            OnPropertyChanged(nameof(Text));
        }
    }
}

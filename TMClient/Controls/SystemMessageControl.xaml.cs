using ClientApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMClient.Utils;
using System.Diagnostics.CodeAnalysis;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для SystemMessageControl.xaml
    /// </summary>
    public partial class SystemMessageControl : MessageBaseControl
    {
        public override IReadOnlyCollection<Message> InnerMessages => [Message];
        public override required User Author { get; init; }
        public override required bool IsOwn { get; set; }
        public override required string Text { get; set; }
        public override required bool IsReaded { get; set; }
        public override required DateTime Time { get; set; }

        public SystemMessage Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        private SystemMessage message=null!;


        [SetsRequiredMembers]
        public SystemMessageControl(SystemMessage message)
        {
            Message = message;
            Author = message.Author;
            IsOwn = message.IsOwn;
            Text = message.Text;
            Time = message.SendTime;
            IsReaded = message.IsReaded;

            DataContext = this;
            InitializeComponent();
        }


        public override bool IsCanUnion(Message newMessage)
        {
            return false;
        }
        public override bool IsCanUnion(MessageBaseControl second)
        {
            return false;
        }
        public override void UnionToEnd(Message message)
        {
            return;
        }

        public override void UnionToStart(Message message)
        {
            return;
        }
    }
}

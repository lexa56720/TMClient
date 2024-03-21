using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TMClient.Utils;

namespace TMClient.Controls
{
    public abstract class MessageBaseControl:UserControl
    {
        public abstract required User Author { get; init; }
        public abstract required bool IsOwn { get; set; }
        public abstract required string Text { get; set; }
        public abstract required bool IsReaded { get; set; }
        public abstract required DateTime Time { get; set; }


        public MessageBaseControl() 
        {
            DataContext = this;
        }
        public abstract IReadOnlyCollection<Message> InnerMessages { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract bool IsCanUnion(Message newMessage);

        public abstract bool IsCanUnion(MessageBaseControl second);

        public abstract void UnionToEnd(Message message);

        public abstract void UnionToStart(Message message);
    }
}

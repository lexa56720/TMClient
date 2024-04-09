using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Utils;

namespace TMClient.Controls
{
    public abstract class MessageBaseControl : UserControl
    {

        public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register(nameof(Message),
                                    typeof(Message),
                                    typeof(MessageBaseControl),
                                    new PropertyMetadata(null));

        public Message Message
        {
            get
            {
                return (Message)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
            }
        }
        public MessageBaseControl()
        {
        }
    }
}

using ApiWrapper.Interfaces;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для MessageControl.xaml
    /// </summary>
    public partial class MessageControl : MessageBaseControl
    {

        public static readonly DependencyProperty OpenImageCommandProperty =
        DependencyProperty.Register(nameof(OpenImageCommand),
                                    typeof(ICommand),
                                    typeof(MessageControl),
                                    new PropertyMetadata(null));
        public ICommand OpenImageCommand
        {
            get
            {
                return (ICommand)GetValue(OpenImageCommandProperty);
            }
            set
            {
                SetValue(OpenImageCommandProperty, value);
            }
        }


        public static readonly DependencyProperty IsAuthorVisibleProperty =
        DependencyProperty.Register(nameof(IsAuthorVisible),
                                    typeof(bool),
                                    typeof(MessageControl),
                                    new PropertyMetadata(true));
        public bool IsAuthorVisible
        { 
            get
            {
                return (bool)GetValue(IsAuthorVisibleProperty);
            }
            set
            {
                SetValue(IsAuthorVisibleProperty, value);
            }
        }


        public MessageControl()
        {
            InitializeComponent();
        }
    }
}

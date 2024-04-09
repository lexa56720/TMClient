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
        public bool IsAuthorVisible
        {
            get => isAuthorVisibile;
            set
            {
                isAuthorVisibile = value;
                OnPropertyChanged(nameof(IsAuthorVisible));
            }
        }
        private bool isAuthorVisibile =false;

        public MessageControl(Message message):base(message)
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}

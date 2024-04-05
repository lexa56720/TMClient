using ApiWrapper.Interfaces;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using TMClient.Model;
using TMClient.Utils;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для MessageControl.xaml
    /// </summary>
    public partial class MessageControl : MessageBaseControl
    {
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

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


        [SetsRequiredMembers]
        public MessageControl(Message message):base(message)
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TMClient.Controls;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для ChatView.xaml
    /// </summary>
    public partial class ChatView : Page
    {
        public ObservableCollection<MessageControl> Messages { get; set; } = new();
        public ChatView()
        {
            InitializeComponent();
            DataContext = this;
            for (int i = 0; i < 20; i++)
                Messages.Add(new MessageControl()
                {
                    IsOwn = Random.Shared.NextDouble()>0.5f,
                    Time="сегоддня",
                    Text = new string('F', 100)
                });
        }
    }
}

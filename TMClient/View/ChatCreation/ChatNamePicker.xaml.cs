﻿using System;
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
using TMClient.Controls;
using TMClient.ViewModel;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для ChatCreation.xaml
    /// </summary>
    public partial class ChatNamePicker : Page
    {
        public Chat? Chat { get; private set; }
        public ChatNamePicker(User[] users,Action<Chat?> dialogCompleted)
        {
            DataContext = new ChatNamePickerViewModel(users, dialogCompleted);
            InitializeComponent();
        }

    }
}

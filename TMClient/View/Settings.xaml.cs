﻿using System.Windows.Controls;
using TMClient.ViewModel;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings(Action openPreviousPage)
        {
            InitializeComponent();
            DataContext = new SettingsViewModel(openPreviousPage);
        }
        public Settings()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}

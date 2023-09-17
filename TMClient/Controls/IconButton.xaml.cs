﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для IconButton.xaml
    /// </summary>
    public partial class IconButton : Button, INotifyPropertyChanged
    {
        public Visibility TextVisibility
        {
            get => textVisibility;
            set
            {
                textVisibility = value;
                OnPropertyChanged(nameof(TextVisibility));
            }

        }
        private Visibility textVisibility = Visibility.Collapsed;

        public Orientation Orientation
        {
            get => orientation;
            set
            {
                orientation = value;
                OnPropertyChanged(nameof(Orientation));
            }
        }
        private Orientation orientation = Orientation.Horizontal;
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconButton),
            new PropertyMetadata(string.Empty, TextPropertyChanged));

        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                OnPropertyChanged(nameof(Icon));

            }
        }
        private string icon = string.Empty;


        public event PropertyChangedEventHandler? PropertyChanged;

        public IconButton()
        {
            InitializeComponent();
        }


        private void TextPropertyChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
                TextVisibility = Visibility.Collapsed;
            else
                TextVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(Text));
        }

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IconButton)d).TextPropertyChanged((string)e.NewValue);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}

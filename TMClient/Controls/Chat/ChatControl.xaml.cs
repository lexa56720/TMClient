﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Utils;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {

        public static readonly DependencyProperty OpenImageCommandProperty =
        DependencyProperty.Register(nameof(OpenImageCommand),
                                    typeof(ICommand),
                                    typeof(ChatControl),
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

        public void SetChatBoxFocus()
        {
            MessageBox.Focus();
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(ChatControl),
            new PropertyMetadata(false));
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsCanReadNewMessagesProperty =
        DependencyProperty.Register(
            nameof(IsCanReadNewMessages),
            typeof(bool),
            typeof(ChatControl),
            new PropertyMetadata(false));

        public bool IsCanReadNewMessages
        {
            get { return (bool)GetValue(IsCanReadNewMessagesProperty); }
            set { SetValue(IsCanReadNewMessagesProperty, value); }
        }


        public static readonly DependencyProperty LoadMoreProperty =
        DependencyProperty.Register(
           nameof(LoadMore),
           typeof(ICommand),
           typeof(ChatControl),
           new PropertyMetadata(null));
        public ICommand LoadMore
        {
            get { return (ICommand)GetValue(LoadMoreProperty); }
            set { SetValue(LoadMoreProperty, value); }
        }

        public static readonly DependencyProperty SendCommandProperty =
        DependencyProperty.Register(
           nameof(SendCommand),
           typeof(ICommand),
           typeof(ChatControl),
           new PropertyMetadata(null));
        public ICommand SendCommand
        {
            get { return (ICommand)GetValue(SendCommandProperty); }
            set { SetValue(SendCommandProperty, value); }
        }

        public static readonly DependencyProperty WrittenTextProperty =
        DependencyProperty.Register(
           nameof(WrittenText),
           typeof(string),
           typeof(ChatControl),
           new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string WrittenText
        {
            get { return (string)GetValue(WrittenTextProperty); }
            set { SetValue(WrittenTextProperty, value); }
        }


        public static readonly DependencyProperty FilesProperty =
        DependencyProperty.Register(nameof(Files),
                                    typeof(ObservableCollection<string>),
                                    typeof(ChatControl));
        public ObservableCollection<string> Files
        {
            get => (ObservableCollection<string>)GetValue(FilesProperty);
            set => SetValue(FilesProperty, value);
        }


        public static readonly DependencyProperty MessagesProperty =
        DependencyProperty.Register(nameof(Messages),
                            typeof(ObservableCollection<MessageContainer>),
                            typeof(ChatControl));
        public ObservableCollection<MessageContainer> Messages
        {
            get => (ObservableCollection<MessageContainer>)GetValue(MessagesProperty);
            set => SetValue(MessagesProperty, value);
        }



        public ICommand AttachCommand => new Command(Attach);
        public ICommand RemoveFileCommand => new Command(RemoveFile);

        public ChatControl()
        {
            SetValue(FilesProperty, new ObservableCollection<string>());
            SetValue(MessagesProperty, new ObservableCollection<MessageContainer>());

            InitializeComponent();
        }

        private bool IsFirstTimeLoad = true;


        private void ChatLoaded(object sender, EventArgs e)
        {
            if (LoadMore.CanExecute(null) && IsFirstTimeLoad)
            {
                IsFirstTimeLoad = false;
                LoadMore.Execute(null);
            }
        }
        private void RemoveFile(object? obj)
        {
            if (obj is not string path)
                return;
            Files.Remove(path);
        }
        public void Attach()
        {
            var files = PathPicker.PickFiles("Все|*.*", true);
            Files.Clear();
            foreach (var file in files)
                Files.Add(file);
        }

    }
}

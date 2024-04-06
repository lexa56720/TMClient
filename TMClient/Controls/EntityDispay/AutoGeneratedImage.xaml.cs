﻿using ClientApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для AutoGeneratedAvatar.xaml
    /// </summary>
    public partial class AutoGeneratedImage : UserControl, INotifyPropertyChanged
    {
        private readonly static Dictionary<int, Color> Colors = new()
        {
            {0,(Color)ColorConverter.ConvertFromString("#F44336") },
            {1,(Color)ColorConverter.ConvertFromString("#E91E63") },
            {2,(Color)ColorConverter.ConvertFromString("#9C27B0") },
            {3,(Color)ColorConverter.ConvertFromString("#673AB7") },
            {4,(Color)ColorConverter.ConvertFromString("#3F51B5") },
            {5,(Color)ColorConverter.ConvertFromString("#00BCD4") },
            {6,(Color)ColorConverter.ConvertFromString("#009688") },
            {7,(Color)ColorConverter.ConvertFromString("#4CAF50") },
            {8,(Color)ColorConverter.ConvertFromString("#FDD835") },
            {9,(Color)ColorConverter.ConvertFromString("#FFC107") },
            {10,(Color)ColorConverter.ConvertFromString("#FF9800") },
            {11,(Color)ColorConverter.ConvertFromString("#FF5722") },
        };

        public static readonly DependencyProperty EntityProperty =
        DependencyProperty.Register(nameof(Entity),
                                    typeof(NamedImageEntity),
                                    typeof(AutoGeneratedImage),
                                    new PropertyMetadata(null, UserChanged));
        public NamedImageEntity Entity
        {
            get => (NamedImageEntity)GetValue(EntityProperty);
            set
            {
                SetValue(EntityProperty, value);
            }
        }

        public SolidColorBrush Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        private SolidColorBrush color=new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public AutoGeneratedImage()
        {
            InitializeComponent();
        }

        private static void UserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoGeneratedImage)d).EntityChanged((NamedImageEntity)e.NewValue);
        }

        private void EntityChanged(NamedImageEntity entity)
        {
            Color = PickColor(entity);
        }

        private SolidColorBrush PickColor(NamedImageEntity entity)
        {
            if (entity == null)
                return new SolidColorBrush(Colors[0]);
            return new SolidColorBrush(Colors[entity.Id % Colors.Count]);
        }
    }
}
using ClientApiWrapper.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApiWrapper
{
    internal class OrderedObservableCollection<T> : ObservableCollection<T>
    {
        private readonly IComparer<T> Comparer;
        private readonly Func<T, INotifyPropertyChanged> Selector;
        private readonly string KeyPropetyName;

        public OrderedObservableCollection(IComparer<T> comparer, Func<T, INotifyPropertyChanged> selector, string keyPropetyName)
        {
            Comparer = comparer;
            Selector = selector;
            KeyPropetyName = keyPropetyName;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Add)
                HandleNewItems(e.NewItems);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                HandleOldItems(e.OldItems);


            if (e.Action == NotifyCollectionChangedAction.Replace || e.Action == NotifyCollectionChangedAction.Reset)
            {
                HandleNewItems(e.NewItems);
                HandleOldItems(e.OldItems);
            }

        }
        private void HandleNewItems(IList? items)
        {
            if (items != null)
            {
                foreach (var item in items)
                    Selector((T)item).PropertyChanged += OnItemPropertyChanged;
                Reorder();
            }
        }
        private void HandleOldItems(IList? items)
        {
            if (items != null)
                foreach (var item in items)
                    Selector((T)item).PropertyChanged -= OnItemPropertyChanged;
        }

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == KeyPropetyName)
                Reorder();
        }

        private void Reorder()
        {
            var orderedCollection = this.OrderDescending(Comparer).ToArray();

            for (int i = 0; i < Count; i++)
            {
                if (Comparer.Compare(this[i], orderedCollection[i]) != 0)
                {
                    RemoveAt(i);
                    Insert(i, orderedCollection[i]);
                }
            }
        }
    }

    public class ChatComparer : IComparer<Chat>
    {
        public int Compare(Chat? x, Chat? y)
        {
            if (x == null && y == null)
                return 0;

            if (x?.LastMessage != null && y?.LastMessage == null)
                return 1;

            if (x?.LastMessage == null && y?.LastMessage != null)
                return -1;

            if (x?.LastMessage == null && y?.LastMessage == null)
                return 0;

            if (x == y || x.LastMessage.SendTime == y.LastMessage.SendTime)
                return 0;

            if (x.LastMessage.SendTime > y.LastMessage.SendTime)
                return 1;
            else
                return -1;
        }
    }

    public class FriendComparer : IComparer<Friend>
    {
        public int Compare(Friend? x, Friend? y)
        {
            var chatComparer = new ChatComparer();
            return chatComparer.Compare(x?.Dialogue, y?.Dialogue);
        }
    }

}

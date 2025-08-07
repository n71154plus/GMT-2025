using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

public class ObservableKeyedCollection<TKey, TValue> : ObservableCollection<TValue>
    where TKey : notnull
    where TValue : INotifyPropertyChanged
    {
    private readonly Func<TValue, TKey> _keySelector;
    private readonly Dictionary<TKey, TValue> _dictionary = new();

    public ObservableKeyedCollection(Func<TValue, TKey> keySelector)
        {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        }

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

    public TValue this[TKey key] => _dictionary[key];

    protected override void InsertItem(int index, TValue item)
        {
        var key = _keySelector(item);
        if (_dictionary.ContainsKey(key))
            throw new ArgumentException($"An item with the same key already exists: {key}");

        base.InsertItem(index, item);
        _dictionary[key] = item;

        SubscribeToItem(item);
        }

    protected override void SetItem(int index, TValue item)
        {
        var oldItem = this[index];
        var oldKey = _keySelector(oldItem);
        var newKey = _keySelector(item);

        if (!EqualityComparer<TKey>.Default.Equals(oldKey, newKey))
            {
            if (_dictionary.ContainsKey(newKey))
                throw new ArgumentException($"An item with the same key already exists: {newKey}");
            _dictionary.Remove(oldKey);
            }

        base.SetItem(index, item);
        _dictionary[newKey] = item;

        UnsubscribeFromItem(oldItem);
        SubscribeToItem(item);
        }

    protected override void RemoveItem(int index)
        {
        var item = this[index];
        var key = _keySelector(item);
        _dictionary.Remove(key);
        base.RemoveItem(index);

        UnsubscribeFromItem(item);
        }

    protected override void ClearItems()
        {
        foreach (var item in this)
            UnsubscribeFromItem(item);

        base.ClearItems();
        _dictionary.Clear();
        }

    private void SubscribeToItem(TValue item)
        {
        item.PropertyChanged += OnItemPropertyChanged;
        }

    private void UnsubscribeFromItem(TValue item)
        {
        item.PropertyChanged -= OnItemPropertyChanged;
        }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
        if (sender is not TValue item) return;

        // 如果 Key 所對應的屬性有變化，更新 Dictionary
        if (_keySelector(item) is TKey newKey)
            {
            // 找舊的 key
            var index = IndexOf(item);
            if (index == -1) return;

            var oldKey = default(TKey);
            foreach (var kv in _dictionary)
                {
                if (EqualityComparer<TValue>.Default.Equals(kv.Value, item))
                    {
                    oldKey = kv.Key;
                    break;
                    }
                }

            if (!EqualityComparer<TKey>.Default.Equals(oldKey, newKey))
                {
                if (_dictionary.ContainsKey(newKey))
                    throw new InvalidOperationException($"Cannot change key to an existing key: {newKey}");

                _dictionary.Remove(oldKey!);
                _dictionary[newKey] = item;

                // 通知 UI 整體內容有變動（如有需要）
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, item, index));
                }
            }
        }
    }

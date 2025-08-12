using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Xunit;

public class ObservableDictionaryTests
{
    [Fact]
    public void Add_RaisesCollectionAndPropertyChanged()
    {
        var dict = new ObservableDictionary<string, int>();
        NotifyCollectionChangedEventArgs collectionArgs = null;
        var propertyNames = new List<string>();
        dict.CollectionChanged += (s, e) => collectionArgs = e;
        dict.PropertyChanged += (s, e) => propertyNames.Add(e.PropertyName);

        dict.Add("a", 1);

        Assert.Equal(NotifyCollectionChangedAction.Add, collectionArgs?.Action);
        Assert.Contains(nameof(dict.Count), propertyNames);
        Assert.Contains(nameof(dict.Keys), propertyNames);
        Assert.Contains(nameof(dict.Values), propertyNames);
    }

    [Fact]
    public void Remove_RaisesCollectionAndPropertyChanged()
    {
        var dict = new ObservableDictionary<string, int>();
        dict.Add("a", 1);

        NotifyCollectionChangedEventArgs collectionArgs = null;
        var propertyNames = new List<string>();
        dict.CollectionChanged += (s, e) => collectionArgs = e;
        dict.PropertyChanged += (s, e) => propertyNames.Add(e.PropertyName);

        dict.Remove("a");

        Assert.Equal(NotifyCollectionChangedAction.Remove, collectionArgs?.Action);
        Assert.Contains(nameof(dict.Count), propertyNames);
        Assert.Contains(nameof(dict.Keys), propertyNames);
        Assert.Contains(nameof(dict.Values), propertyNames);
    }

    [Fact]
    public void Clear_RaisesCollectionResetAndPropertyChangedCount()
    {
        var dict = new ObservableDictionary<string, int>();
        dict.Add("a", 1);

        NotifyCollectionChangedEventArgs collectionArgs = null;
        var propertyNames = new List<string>();
        dict.CollectionChanged += (s, e) => collectionArgs = e;
        dict.PropertyChanged += (s, e) => propertyNames.Add(e.PropertyName);

        dict.Clear();

        Assert.Equal(NotifyCollectionChangedAction.Reset, collectionArgs?.Action);
        Assert.Single(propertyNames);
        Assert.Contains(nameof(dict.Count), propertyNames);
    }

    [Fact]
    public void UpdatingExistingKey_RaisesReplaceAction()
    {
        var dict = new ObservableDictionary<string, int>();
        NotifyCollectionChangedAction? secondAction = null;

        dict.CollectionChanged += (s, e) => secondAction = e.Action;

        dict["a"] = 1;
        secondAction = null;
        dict["a"] = 2;

        Assert.Equal(NotifyCollectionChangedAction.Replace, secondAction);
    }
}

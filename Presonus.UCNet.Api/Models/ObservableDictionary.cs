using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Presonus.UCNet.Api.Models
{
	public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public event PropertyChangedEventHandler PropertyChanged;

		public new TValue this[TKey key]
		{
			get => base[key];
			set
			{
				if (TryGetValue(key, out TValue oldValue))
				{
					base[key] = value;
					OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, oldValue));
				}
				else
				{
					Add(key, value);
				}
			}
		}

		protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> item)
		{
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item));
			OnPropertyChanged(nameof(Count));
		}

		protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
		{
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
			OnPropertyChanged(nameof(Count));
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public new void Add(TKey key, TValue value)
		{
			base.Add(key, value);
			OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
		}

		public new bool Remove(TKey key)
		{
			if (base.Remove(key, out TValue value))
			{
				OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));
				return true;
			}

			return false;
		}
	}
}
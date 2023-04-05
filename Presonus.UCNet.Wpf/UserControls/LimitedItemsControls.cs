using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Presonus.UCNet.Wpf.UserControls
{
	public class LimitedItemsControl : ItemsControl
	{
		public static readonly DependencyProperty MaxItemsProperty =
			DependencyProperty.Register(nameof(MaxItems), typeof(int), typeof(LimitedItemsControl), new PropertyMetadata(int.MaxValue));

		public int MaxItems
		{
			get { return (int)GetValue(MaxItemsProperty); }
			set { SetValue(MaxItemsProperty, Math.Max(value, 0)); }
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);

			if (Items?.Count > MaxItems)
			{
				RemoveExcessItems();
			}
		}

		private void RemoveExcessItems()
		{
			var itemsToRemove = Items.Cast<object>().Skip(MaxItems).ToList();
			foreach (var item in itemsToRemove)
			{
				Items.Remove(item);
			}
		}

		public new void Add(object item)
		{
			if (Items.Count < MaxItems)
			{
				Items.Add(item);
			}
		}
	}

}

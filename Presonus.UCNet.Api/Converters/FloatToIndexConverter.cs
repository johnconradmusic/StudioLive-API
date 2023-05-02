using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Presonus.UCNet.Api.Converters
{
	public class FloatToIndexConverter : IMultiValueConverter
	{
		private int _itemCount;

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values[0] == DependencyProperty.UnsetValue || (float)values[0] > 1) return 0;
			if (values == null || values.Length < 2)
				return -1;

			float floatValue = 0;
			if (values[0].GetType() == typeof(float))
				floatValue = (float)values[0];
			else
				floatValue = (int)values[0];
			IList itemList = values[1] as IList;
			_itemCount = itemList?.Count ?? 1;

			return (int)Math.Round(floatValue * (_itemCount - 1));
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			if (value == null)
				return new object[] { 0f };
			float selectedIndex = 0;
			if (value.GetType() == typeof(float))
				selectedIndex = (float)value;
			else
				selectedIndex = (int)value;

			return new object[] { selectedIndex / (_itemCount - 1) };
		}
	}

}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Reflection;

namespace Presonus.UCNet.Wpf.Converters
{
	public class VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || value.Equals(DependencyProperty.UnsetValue))
			{
				//Console.WriteLine($"VisibilityConverter.Convert: value is null or UnsetValue. Returning Collapsed.");
				return Visibility.Collapsed;
			}

			if (value is PropertyInfo property && property.GetValue(value) == null)
			{
				//Console.WriteLine($"VisibilityConverter.Convert: property {property.Name} is null. Returning Collapsed.");
				return Visibility.Collapsed;
			}

			//Console.WriteLine($"VisibilityConverter.Convert: value {value} is not null or UnsetValue, and property is not null. Returning Visible.");
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}



}

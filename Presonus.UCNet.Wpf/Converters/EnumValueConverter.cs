using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Presonus.UCNet.Wpf.Converters
{
	public class EnumValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
			{
				return null;
			}

			var enumValue = (Enum)value;
			var enumType = (Type)parameter;
			var values = Enum.GetValues(enumType);
			var index = Array.IndexOf(values, enumValue);

			// Handle negative index values
			if (index < 0)
			{
				index = 0;
			}

			return (float)index / (values.Length - 1);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
			{
				return null;
			}

			var floatValue = (float)value;
			var enumType = (Type)parameter;
			var values = Enum.GetValues(enumType);
			var index = (int)Math.Round(floatValue * (values.Length - 1));

			// Handle out-of-range index values
			if (index < 0 || index >= values.Length)
			{
				index = values.Length - 1;
			}

			return values.GetValue(index);
		}
	}


}

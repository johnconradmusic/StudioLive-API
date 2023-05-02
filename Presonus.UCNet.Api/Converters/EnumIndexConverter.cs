using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Presonus.UCNet.Api.Converters
{
	public class EnumIndexConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
			{
				return null;
			}

			var enumType = (Type)parameter;
			var values = Enum.GetValues(enumType);
			var index = (int)Math.Round((float)value * (values.Length - 1));
			return values.GetValue(index);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
			{
				return null;
			}

			var enumType = (Type)parameter;
			var values = Enum.GetValues(enumType);
			var index = Array.IndexOf(values, value);
			return (float)index / (values.Length - 1);
		}
	}

}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Presonus.UCNet.Api.Converters
{
	public class OpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() == typeof(float))
			{
				double opacity = (float)value;
				double threshold = double.Parse((string)parameter, CultureInfo.InvariantCulture);

				if (opacity >= threshold)
				{
					return 1.0;
				}
				else
				{
					return 0;
				}
			}
			else if (value.GetType() == typeof(bool))
			{
				bool state = (bool)value;
				if (state == true)
				{
					return 1.0;
				}
				return 0;
			}
			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

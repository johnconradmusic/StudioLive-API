
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Presonus.UCNet.Api.Converters
{
	public class HexStringToSolidColorBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || string.IsNullOrEmpty(value.ToString()))
			{
				return new SolidColorBrush(Colors.HotPink);
			}

			string hexColor = (string)value;

			if (hexColor == null)
			{
				return null;
			}

			if (hexColor.StartsWith("#"))
			{
				hexColor = hexColor.Substring(1);
			}

			int r = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
			int g = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
			int b = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
			if (r == 0 && g == 0 && b == 0) return null;
			var result = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}

}

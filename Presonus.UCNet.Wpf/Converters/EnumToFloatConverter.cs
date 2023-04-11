using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Presonus.UCNet.Wpf.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;
	using System.Diagnostics;

	public class EnumToFloatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Console.WriteLine($"EnumToFloatConverter - Convert: value={value}, targetType={targetType}, parameter={parameter}");

			if (value == null || parameter == null)
			{
				Console.WriteLine($"EnumToFloatConverter - Convert: value or parameter is null, returning 0");
				return 0;
			}
			float floatValue = 0;
			if (value.GetType() == typeof(int))
			{
				floatValue = (int)value;
			}
			else
			{
				floatValue = (float)value;
			}

			int enumCount = Enum.GetValues(parameter as Type).Length;

			float result = (float)floatValue * (enumCount - 1);
			Console.WriteLine($"EnumToFloatConverter - Convert: selectedIndex={floatValue}, enumCount={enumCount}, result={result}");

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Console.WriteLine($"EnumToFloatConverter - ConvertBack: value={value}, targetType={targetType}, parameter={parameter}");

			if (value == null || parameter == null)
			{
				Console.WriteLine($"EnumToFloatConverter - ConvertBack: value or parameter is null, returning 0f");
				return 0f;
			}

			float selectedIndex = (int)value;
			int enumCount = Enum.GetValues(parameter as Type).Length;

			float result = selectedIndex / (enumCount - 1);
			Console.WriteLine($"EnumToFloatConverter - ConvertBack: floatValue={selectedIndex}, enumCount={enumCount}, result={result}");

			return result;
		}
	}


}

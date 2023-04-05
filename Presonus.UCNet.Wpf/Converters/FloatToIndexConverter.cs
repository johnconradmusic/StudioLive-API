using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
namespace Presonus.UCNet.Wpf.Converters
{
	public class FloatToEnumValuesConverter<T> : IValueConverter where T : Enum
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> enumValues = new List<string>();
			int selectedIndex = 0;
			if (value is float floatValue)
			{
				// Get the minimum and maximum values of the Enum entries.
				T[] values = (T[])Enum.GetValues(typeof(T));
				T min = values.Min();
				T max = values.Max();

				// Calculate the range of the Enum values.
				int range = System.Convert.ToInt32(max) - System.Convert.ToInt32(min);

				// Scale the float value to the range of the Enum values.
				int scaledValue = System.Convert.ToInt32(floatValue * range);
				int enumValue = System.Convert.ToInt32(min) + scaledValue;

				// Get the string values of the Enum entries and the selected index.
				for (int i = 0; i < values.Length; i++)
				{
					string enumString = values[i].ToString();
					enumValues.Add(enumString);
					if (System.Convert.ToInt32(values[i]) == enumValue)
					{
						selectedIndex = i;
					}
				}
			}

			// Return the list of string values and the selected index.
			return new Tuple<List<string>, int>(enumValues, selectedIndex);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Tuple<List<string>, int> enumTuple)
			{
				// Get the Enum value from the selected index.
				T[] values = (T[])Enum.GetValues(typeof(T));
				T selectedValue = values[enumTuple.Item2];

				// Convert the Enum value to a float value between 0 and 1.
				float range = Enum.GetValues(typeof(T)).Length - 1;
				float scaledValue = (float)(System.Convert.ToInt32(selectedValue) - System.Convert.ToInt32(values.Min())) / range;
				return scaledValue;
			}
			return Binding.DoNothing;
		}
	}


}

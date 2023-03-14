//------------------------------------------------------------------------------
// The Assistant - Copyright (c) 2016-2023, John Conrad
//------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.Windows.Data;

namespace Presonus.StudioLive32.Wpf.Converters
{
	public class AlwaysVisibleConverter : IValueConverter
	{
		public object Convert(object value,
							  Type targetType, object parameter, CultureInfo culture)
		{
			return true;
		}

		public object ConvertBack(object value, Type targetType,
								  object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
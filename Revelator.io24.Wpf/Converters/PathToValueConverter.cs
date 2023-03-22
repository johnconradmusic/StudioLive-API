using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Presonus.StudioLive32.Wpf.Converters
{
	public class PathToValueConverter : IValueConverter
	{
		private readonly MixerStateService _mixerStateService;

		public PathToValueConverter(MixerStateService mixerStateService)
		{
			_mixerStateService = mixerStateService;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string path)
			{
				return _mixerStateService.GetValue(path);
			}

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

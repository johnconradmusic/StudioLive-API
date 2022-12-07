using Presonus.StudioLive32.Wpf.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Presonus.StudioLive32.Wpf.Theme
{
	public partial class FaderStyle
	{
		private void Focused(object sender, RoutedEventArgs e)
		{
			Mixer.ReportValueOfControl((Control)sender, true);

		}

		private void ValueChanged(object sender, RoutedEventArgs e)
		{
			Mixer.ReportValueOfControl((Control)sender);
		}

		private void TextChanged(object sender, RoutedEventArgs e)
		{

		}
	}

	public partial class PanPotStyle
	{
		private void ValueChanged(object sender, RoutedEventArgs e)
		{
			Mixer.ReportValueOfControl((PanPot)sender);
		}

		private void TextChanged(object sender, RoutedEventArgs e)
		{

		}
	}
}

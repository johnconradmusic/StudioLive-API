using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Presonus.UCNet.Wpf.Blind.UserControls
{
	public class CustomMenuItem : MenuItem
	{
		public CustomMenuItem()
		{
			GotFocus += CustomMenuItem_GotFocus;
		}

		private void CustomMenuItem_GotFocus(object sender, System.Windows.RoutedEventArgs e)
		{
			if (IsCheckable)
			{
				string checkState = IsChecked ? "on" : "off";
				Speech.SpeechManager.Say($"{Header} ({checkState})");
			}
			else
				Speech.SpeechManager.Say(Header);
		}
	}
}

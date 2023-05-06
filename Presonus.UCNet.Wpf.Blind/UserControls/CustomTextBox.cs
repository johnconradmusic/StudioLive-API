using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.UCNet.Wpf.Blind.UserControls
{
	internal class CustomTextBox : TextBox
	{
		public CustomTextBox()
		{
			Loaded += CustomTextBox_Loaded;
			PreviewKeyDown += CustomTextBox_PreviewKeyDown;
			TextChanged += CustomTextBox_TextChanged;
			
		}

		private void CustomTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			Speech.SpeechManager.Say(Text, false);
		}

		private void CustomTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			// Check if the key pressed is alphanumeric
			if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
			{
				// If the key pressed is not alphanumeric, mark the event as handled
				Speech.SpeechManager.Say(e.Key);
			}
			if (e.Key == Key.Left || e.Key == Key.Right)
			{
				int caretIndex = CaretIndex;
				if(caretIndex < Text.Length)
				Speech.SpeechManager.Say(Text[caretIndex]);
			}
		}

		private void CustomTextBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			
		}
	}
}

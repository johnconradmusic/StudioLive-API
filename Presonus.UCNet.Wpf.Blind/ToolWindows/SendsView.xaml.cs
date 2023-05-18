using Presonus.UCNet.Api;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Wpf.Blind.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
	/// <summary>
	/// Interaction logic for SendsView.xaml
	/// </summary>
	public partial class SendsView : ToolWindow
	{
		BlindViewModel blindViewModel;
		public SendsView(Channel channel, BlindViewModel viewModel) : base(channel)
		{
			blindViewModel = viewModel;
			InitializeComponent();
			Title = $"Sends Window - {channel.chnum} ({channel.username})";
			Loaded += SendsView_Loaded;
		}

		private void SendsView_Loaded(object sender, RoutedEventArgs e)
		{
			Console.WriteLine(Mixer.ChannelCounts[ChannelTypes.AUX]);
			for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.AUX]; i++)
			{
				var panel = new StackPanel();
				bool linkmaster = blindViewModel.Auxes[i].linkmaster;
				if (linkmaster) //stereo pair odd + even
				{
					var sendVolume = new NumericUpDown();
					sendVolume.Curve = Api.Helpers.CurveFormula.LinearToVolume;
					sendVolume.Unit = Api.Helpers.Units.DB;
					sendVolume.Min = -84;
					sendVolume.Max = 10;
					sendVolume.Default = 0.735f;
					sendVolume.Caption = blindViewModel.Auxes[i].username + " level";
					sendVolume.SetBinding(NumericUpDown.ValueProperty, new Binding("aux" + (i + 1)));

					panel.Children.Add(sendVolume);

					var sendPan = new NumericUpDown();
					sendPan.Curve = Api.Helpers.CurveFormula.Linear;
					sendPan.Unit = Api.Helpers.Units.PAN;
					sendPan.Default = 0.5f;
					sendPan.Min = 0f;
					sendPan.Max = 1f;
					sendPan.Caption = blindViewModel.Auxes[i].username + " pan";
					sendPan.SetBinding(NumericUpDown.ValueProperty, new Binding($"aux{i + 1}{i + 2}_pan"));

					panel.Children.Add(sendPan);

				}
				else if (blindViewModel.Auxes[i].link && !blindViewModel.Auxes[i].linkmaster) { }
				else
				{
					var sendVolume = new NumericUpDown();
					sendVolume.Curve = Api.Helpers.CurveFormula.LinearToVolume;
					sendVolume.Unit = Api.Helpers.Units.DB;
					sendVolume.Min = -84;
					sendVolume.Max = 10;
					sendVolume.Default = 0.735f;
					sendVolume.Caption = blindViewModel.Auxes[i].username + " level";
					sendVolume.SetBinding(NumericUpDown.ValueProperty, new Binding("aux" + (i + 1)));

					panel.Children.Add(sendVolume);

				}

				sendsPanel.Children.Add(panel);

			}
			var newPanel = new StackPanel();
			for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.FX]; i++)
			{
				var fx1Volume = new NumericUpDown();
				fx1Volume.Curve = Api.Helpers.CurveFormula.LinearToVolume;
				fx1Volume.Unit = Api.Helpers.Units.DB;
				fx1Volume.Min = -84;
				fx1Volume.Max = 10;
				fx1Volume.Default = 0.735f;
				fx1Volume.Caption = blindViewModel.FX[i].Name;
				fx1Volume.SetBinding(NumericUpDown.ValueProperty, new Binding("FX" + IntToLetter(i + 1)));

				newPanel.Children.Add(fx1Volume);
			}
			sendsPanel.Children.Add(newPanel);
		}
		public string IntToLetter(int number)
		{
			// Check if the number is within the valid range (1-26)
			if (number < 1 || number > 26)
			{
				throw new ArgumentOutOfRangeException(nameof(number), "The number must be between 1 and 26.");
			}

			// Convert number to ASCII value of the equivalent uppercase letter
			// 'A' in ASCII is 65, so we add 64 to the input number
			char letter = (char)(number + 64);

			// Return the letter as a string
			return letter.ToString();
		}
	}
}

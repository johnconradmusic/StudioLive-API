using Presonus.UCNet.Api;
using Presonus.UCNet.Api.Models;
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
using static System.Security.Cryptography.ECCurve;

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
			for (int i = 1; i <= Mixer.ChannelCounts[ChannelTypes.AUX]; i++)
			{
				var panel = new StackPanel();
				if (blindViewModel.Auxes[i].link && i % 2 != 0)
				{
					var sendVolume = new NumericUpDown();
					sendVolume.Curve = Api.Helpers.CurveFormula.LinearToVolume;
					sendVolume.Unit = Api.Helpers.Units.DB;
					sendVolume.Min = -84;
					sendVolume.Max = 10;
					sendVolume.Default = 0.735f;
					sendVolume.Caption = "Aux " + i;
					sendVolume.SetBinding(NumericUpDown.ValueProperty, new Binding("aux" + i));

					panel.Children.Add(sendVolume);

					//var sendPan = new NumericUpDown();
					//sendPan.Curve = Api.Helpers.CurveFormula.Linear;
					//sendPan.Unit = Api.Helpers.Units.PAN;
					//sendPan.Default = 0.5f;
					//sendPan.Caption = "Aux 1 & 2 Pan";
					//sendPan.SetBinding(NumericUpDown.ValueProperty, new Binding("aux" + i + (i + 1) + "_pan"));

					//panel.Children.Add(sendPan);

					sendsPanel.Children.Add(panel);
				}
			}
		}
	}
}

using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Wpf.Blind.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
	/// <summary>
	/// Interaction logic for RoutingToolWindow.xaml
	/// </summary>
	public partial class RoutingToolWindow : ToolWindow
	{
		private Channel _channel;
		private ListUpDown inputsrc;
		private ListUpDown adcSrc;
		private ListUpDown avbSrc;
		private ListUpDown usbSrc;
		private ListUpDown sdSrc;

		public RoutingToolWindow(Channel channel) : base(channel)
		{
			_channel = channel;
			InitializeComponent();
			Title = $"Sends Window - {channel.chnum} ({channel.username})";
			Loaded += RoutingToolWindow_Loaded;
		}

		private void RoutingToolWindow_Loaded(object sender, RoutedEventArgs e)
		{


			StackPanel panel = new();
			if (_channel is InputChannel lineInput)
			{
				inputsrc = new ListUpDown();
				inputsrc.DataContext = lineInput;
				inputsrc.Caption = "Input Source";
				inputsrc.Items = lineInput.inputsrc_values;
				inputsrc.SetBinding(ListUpDown.ValueProperty, new Binding("inputsrc"));

				panel.Children.Add(inputsrc);

				adcSrc = new ListUpDown();
				adcSrc.Caption = "Analog Source";
				adcSrc.Items = lineInput.adc_src_values;
				adcSrc.SetBinding(ListUpDown.ValueProperty, new Binding("adc_src"));

				panel.Children.Add(adcSrc);

				avbSrc = new ListUpDown();
				avbSrc.Caption = "Network Source";
				avbSrc.Items = lineInput.avb_src_values;
				avbSrc.SetBinding(ListUpDown.ValueProperty, new Binding("avb_src"));

				panel.Children.Add(avbSrc);

				usbSrc = new ListUpDown();
				usbSrc.Caption = "USB Source";
				usbSrc.Items = lineInput.usb_src_values;
				usbSrc.SetBinding(ListUpDown.ValueProperty, new Binding("usb_src"));

				panel.Children.Add(usbSrc);

				sdSrc = new ListUpDown();
				sdSrc.Caption = "SD Source";
				sdSrc.Items = lineInput.sd_src_values;
				sdSrc.SetBinding(ListUpDown.ValueProperty, new Binding("sd_src"));

				panel.Children.Add(sdSrc);
				inputsrc.ValueChanged += Inputsrc_ValueChanged;
				NewMethod(inputsrc.ValueString);

			}
			else if (_channel is FX outputBus)
			{
				var adcSrc = new ListUpDown();
				adcSrc.Caption = "Aux Pre/Post";
				adcSrc.Items = OutputBus.auxpremode_values;
				adcSrc.SetBinding(ListUpDown.ValueProperty, new Binding("auxpremode"));

				panel.Children.Add(adcSrc);
			}
			else if (_channel is OutputDACBus outputDACBus)
			{
				var auxmode = new ListUpDown();
				auxmode.Caption = "Aux Pre/Post";
				auxmode.Items = OutputBus.auxpremode_values;
				auxmode.SetBinding(ListUpDown.ValueProperty, new Binding("auxpremode"));

				panel.Children.Add(auxmode);

				var busmode = new ListUpDown();
				busmode.Caption = "Bus Mode";
				busmode.Items = OutputDACBus.busmode_values;
				busmode.SetBinding(ListUpDown.ValueProperty, new Binding("busmode"));

				panel.Children.Add(busmode);

				var bussource = new ListUpDown();
				bussource.Caption = "Bus Source";
				bussource.Items = OutputDACBus.bussource_values;
				bussource.SetBinding(ListUpDown.ValueProperty, new Binding("bussrc"));

				panel.Children.Add(bussource);

				var lr = new BooleanUpDown();
				lr.Caption = "LR Assign";
				lr.SetBinding(BooleanUpDown.ValueProperty, new Binding("lr_assign"));
			}
			routingPanel.Children.Add(panel);

		}

		private void Inputsrc_ValueChanged(object sender, ListUpDownEventArgs e)
		{
			NewMethod(e.ValueString);
		}

		private void NewMethod(string e)
		{
			adcSrc.Visibility = Visibility.Collapsed;
			avbSrc.Visibility = Visibility.Collapsed;
			usbSrc.Visibility = Visibility.Collapsed;
			sdSrc.Visibility = Visibility.Collapsed;

			switch (e)
			{
				case "Analog":
					adcSrc.Visibility = Visibility.Visible;
					break;
				case "Network":
					avbSrc.Visibility = Visibility.Visible;
					break;
				case "USB":
					usbSrc.Visibility = Visibility.Visible;
					break;
				case "SD":
					sdSrc.Visibility = Visibility.Visible;
					break;
			}
		}
	}
}
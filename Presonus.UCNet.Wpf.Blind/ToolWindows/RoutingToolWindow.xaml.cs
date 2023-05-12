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
			}
			routingPanel.Children.Add(panel);

			inputsrc.ValueChanged += Inputsrc_ValueChanged;
		}

		private void Inputsrc_ValueChanged(object sender, ListUpDownEventArgs e)
		{
			adcSrc.Visibility = Visibility.Collapsed;
			avbSrc.Visibility = Visibility.Collapsed;
			usbSrc.Visibility = Visibility.Collapsed;
			sdSrc.Visibility = Visibility.Collapsed;

			switch (e.ValueString)
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
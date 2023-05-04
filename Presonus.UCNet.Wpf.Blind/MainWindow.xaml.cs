using Microsoft.Extensions.DependencyInjection;
using Presonus.UCNet.Api;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Api.Services;
using Presonus.UCNet.Wpf.Blind.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.UCNet.Wpf.Blind
{
	public partial class MainWindow : Window
	{
		BlindViewModel blindViewModel;
		public MainWindow(BlindViewModel viewModel, Speech.SpeechManager speechManager)
		{
			DataContext = blindViewModel = viewModel;
			InitializeComponent();
			Loaded += (s, e) =>
			{
				_meterService = App.ServiceProvider.GetRequiredService<MeterService>();
				_meterService.MeterDataReceived += _meterService_MeterDataReceived;

				ChannelSelector.SelectedIndex = 0;
			};
		}

		public float Meter => _channel?.ChannelType != null && _channel.ChannelIndex != null
			? _meterService.MeterData.GetData(new(_channel.ChannelType, _channel.ChannelIndex - 1))
			: 0.0f;
		public float Peak;
		private Channel _channel;
		private MeterService _meterService;


		private void _meterService_MeterDataReceived(object? sender, MeterDataEventArgs e)
		{
			var value = ValueTransformer.LinearToMeter(Meter);
			if (value > Peak) Peak = value;
			else Peak *= 0.9f;

			DoClipProtection();
		}

		private void DoClipProtection()
		{
			foreach (var channel in blindViewModel.AllChannels)
			{
				if (channel is MicLineInput input && channel.clipProtection)
				{
					if (_meterService.MeterData.GetData(new(channel.ChannelType, channel.ChannelIndex - 1)) > 0.9f)
					{
						input.preampgain -= 0.1f;
					}
				}
			}
		}
		private void ChannelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ChannelSelector.SelectedItem is Channel channel)
			{
				_channel = channel;
				Speech.SpeechManager.Say($"{channel.chnum} ({channel.username})");
				if (channel.link)
				{
					if (channel.linkmaster)
					{
						Speech.SpeechManager.Say($"Left side of linked pair", false);
					}
					else
					{
						Speech.SpeechManager.Say($"Right side of linked pair", false);
					}
				}
				Console.WriteLine($"Channel {channel.chnum} ({channel.username})");

				// Check if the focused element is an IAccessibleControl
				if (Keyboard.FocusedElement is NumericUpDown focusedControl)
				{
					string valueString = focusedControl.ValueString;
					Speech.SpeechManager.Say(valueString, false);
				}
			}
		}


		private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{

			if (e.Key == Key.M)
			{
				if (UserControls.ModifierKeys.IsCtrlDown())
				{
					Speech.SpeechManager.Say($"{ValueTransformer.LinearToVolume((float)Peak)}");
				}
				else
				{
					_channel.mute = !_channel.mute;
					if (_channel.mute)
						Speech.SpeechManager.Say($"Muted");
					else
						Speech.SpeechManager.Say($"Unmuted");


				}
			}
			if (e.Key == Key.Right)
			{
				e.Handled = true;
				int newIndex = Math.Min(ChannelSelector.SelectedIndex + 1, ChannelSelector.Items.Count - 1);
				ChannelSelector.SelectedIndex = newIndex;
			}
			else if (e.Key == Key.Left)
			{
				e.Handled = true;
				int newIndex = Math.Max(ChannelSelector.SelectedIndex - 1, 0);
				ChannelSelector.SelectedIndex = newIndex;
			}
		}

		private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Apps)
			{
				if (_channel != null)
				{
					contextMenu.DataContext = _channel;
					contextMenu.IsOpen = true;
				}
				e.Handled = true;
			}
		}
	}
}
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Presonus.UCNet.Api;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Api.Services;
using Presonus.UCNet.Wpf.Blind.ToolWindows;
using Presonus.UCNet.Wpf.Blind.UserControls;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModifierKeys = Presonus.UCNet.Wpf.Blind.UserControls.ModifierKeys;

namespace Presonus.UCNet.Wpf.Blind
{
	public partial class MainWindow : Window
	{
		BlindViewModel blindViewModel;
		public MainWindow(BlindViewModel viewModel, Speech.SpeechManager speechManager)
		{
			DataContext = blindViewModel = viewModel;
			InitializeComponent();
			Loaded += MainWindow_Loaded;

			Activated += MainWindow_Activated;
			Title = blindViewModel.Global.devicename;
		}

		private void BuildMixControls(ChannelSelector channelSelector)
		{
			if (_channel is OutputDACBus) return;

			switch (channelSelector.Type)
			{
				case ChannelTypes.MAIN:
					{
						mixPanel.Children.Clear();
						//< usercontrols:NumericUpDown x:Name = "trimControl" Caption = "Trim" Value = "{Binding preampgain}" Curve = "Linear" Min = "0" Max = "60" Unit = "DB" Default = "0" />
						mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Trim", 0, 60, 0, Units.DB, CurveFormula.Linear, "preampgain"));

						//< usercontrols:NumericUpDown x:Name = "volumeControl" Caption = "Level" Value = "{Binding volume}" Curve = "LinearToVolume" Min = "-84" Max = "10" Unit = "DB" Default = "0.735" />
						mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Level", -84, 10, 0.735f, Units.DB, CurveFormula.LinearToVolume, "volume"));

						//< usercontrols:NumericUpDown x:Name = "hipassControl" Caption = "Hi Pass Filter" Value = "{Binding hpf}" Curve = "Logarithmic" Min = "24" Max = "1000" Unit = "HZ_24OFF" Default = "0" />
						mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Hi Pass Filter", 24, 1000, 0, Units.HZ_24OFF, CurveFormula.Logarithmic, "hpf"));

						//< usercontrols:NumericUpDown x:Name = "panControl" Caption = "Pan" Value = "{Binding pan}" Curve = "Linear" Min = "0" Max = "1" Unit = "PAN" Default = "0.5" />
						mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Trim", 0, 1, 0, Units.PAN, CurveFormula.Linear, "pan"));

					}
					break;
				case ChannelTypes.AUX:
					{
						mixPanel.Children.Clear();

						//< usercontrols:NumericUpDown x:Name = "trimControl" Caption = "Trim" Value = "{Binding preampgain}" Curve = "Linear" Min = "0" Max = "60" Unit = "DB" Default = "0" />
						mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Trim", 0, 60, 0, Units.DB, CurveFormula.Linear, "preampgain"));

						//< usercontrols:NumericUpDown x:Name = "volumeControl" Caption = "Level" Value = "{Binding volume}" Curve = "LinearToVolume" Min = "-84" Max = "10" Unit = "DB" Default = "0.735" />
						mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Level", -84, 10, 0.735f, Units.DB, CurveFormula.LinearToVolume, $"aux{channelSelector.Channel}"));

						//< usercontrols:NumericUpDown x:Name = "hipassControl" Caption = "Hi Pass Filter" Value = "{Binding hpf}" Curve = "Logarithmic" Min = "24" Max = "1000" Unit = "HZ_24OFF" Default = "0" />
						mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Hi Pass Filter", 24, 1000, 0, Units.HZ_24OFF, CurveFormula.Logarithmic, "hpf"));

						var chanNum = channelSelector.Channel;
						var auxChan = blindViewModel.Auxes[chanNum - 1];
						if (auxChan.linkmaster)
						{
							mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Pan", 0, 1, 0, Units.PAN, CurveFormula.Linear, $"aux{chanNum}{chanNum + 1}_pan"));

						}
						else if (auxChan.link && !auxChan.linkmaster)
						{
							var channelbefore = blindViewModel.Auxes[channelSelector.Channel - 2];
							mixPanel.Children.Add(ControlFactory.CreateNumericUpDownControl("Pan", 0, 1, 0, Units.PAN, CurveFormula.Linear, $"aux{chanNum - 1}{chanNum}_pan"));

						}

					}
					break;
			}
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			_meterService = App.ServiceProvider.GetRequiredService<MeterService>();
			_meterService.MeterDataReceived += _meterService_MeterDataReceived;

			ChannelSelector.SelectedIndex = 0;
			//var mainChannel = blindViewModel.Main[0];
			//BuildMixControls(new(mainChannel));
			for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.AUX]; i++)
			{
				var auxChannel = blindViewModel.Auxes[i];
				var menuItem = new CustomMenuItem()
				{
					Header = auxChannel.username
				};
				menuItem.Click += (s, e) => BuildMixControls(new ChannelSelector(auxChannel));

				mixMenu.Items.Add(menuItem);
			}
			{
				var mainChannel = blindViewModel.Main[0];
				var menuItem = new CustomMenuItem()
				{
					Header = mainChannel.username
				};
				menuItem.Click += (s, e) => BuildMixControls(new ChannelSelector(mainChannel));

				mixMenu.Items.Add(menuItem);
			}
			for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.GEQ]; i++)
			{
				int index = i;
				var menuItem = new CustomMenuItem()
				{
					Header = "Graphic EQ " + (i + 1)
				};
				menuItem.Click += (s, e) =>
				{
					new GEQWindow(blindViewModel.GEQ[index]).ShowDialog();
				};
				viewMenu.Items.Add(menuItem);
			}

			for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.FX]; i++)
			{
				var letter = "";
				switch (i)
				{
					case 0:
						letter = "A";
						break;
					case 1:
						letter = "B";
						break;
					case 2:
						letter = "C";
						break;
					case 3:
						letter = "D";
						break;
				}
				//< usercontrols:CustomMenuItem Tag = "FXA" Header = "FXA" Click = "CustomMenuItem_Click" />     
				//< usercontrols:CustomMenuItem Tag = "FXB" Header = "FXB" Click = "CustomMenuItem_Click" />
				var menuItem = new CustomMenuItem();
				menuItem.Header = $"FX{letter}";
				menuItem.Tag = $"FX{letter}";
				menuItem.Click += CustomMenuItem_Click;
				viewMenu.Items.Add(menuItem);
			}
		}
		private void MainWindow_Activated(object? sender, EventArgs e)
		{
			Speech.SpeechManager.Say($"{Title} ");
			if (_channel != null)
				Speech.SpeechManager.Say($"Channel {_channel.chnum} selected", false);
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
					var dif = _meterService.MeterData.GetData(new(channel.ChannelType, channel.ChannelIndex - 1)) - 0.75f;
					if (dif > 0)
					{
						trimControl.Value -= dif / 16;
					}
				}
			}
		}
		private void ChannelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ChannelSelector.SelectedItem is Channel channel)
			{
				e.RemovedItems.Cast<Channel>().ToList().ForEach(channel => channel.select = false);
				_channel = channel;
				channel.select = true;
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
			if (Keyboard.FocusedElement is CustomMenuItem)
			{
				return;
			}
			switch (e.Key)
			{
				case Key.V:
					e.Handled = true;
					volumeControl.Focus();
					break;
				case Key.H:
					e.Handled = true;
					hipassControl.Focus();
					break;
				case Key.T:
					e.Handled = true;
					trimControl.Focus();
					break;
				case Key.P:
					e.Handled = true;
					panControl.Focus();
					break;
				case Key.G:
					e.Handled = true;
					new GateToolWindow(_channel).ShowDialog();
					break;

				case Key.E:
					e.Handled = true;
					new EQ4ToolWindow(_channel).ShowDialog();
					break;

				case Key.C:
					e.Handled = true;
					new CompToolWindow(_channel).ShowDialog();
					break;

				case Key.S:
					new SendsView(_channel, blindViewModel).ShowDialog();
					break;
				case Key.M:
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
					break;

				case Key.Right:
					e.Handled = true;
					int newIndex = Math.Min(ChannelSelector.SelectedIndex + 1, ChannelSelector.Items.Count - 1);
					ChannelSelector.SelectedIndex = newIndex;
					break;

				case Key.Left:
					e.Handled = true;
					newIndex = Math.Max(ChannelSelector.SelectedIndex - 1, 0);
					ChannelSelector.SelectedIndex = newIndex;
					break;
				case Key.F:
					if (ModifierKeys.IsCtrlDown())
					{
						var dialog = new ChannelSelectorToolWindow(blindViewModel);
						dialog.ShowDialog();

						if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
						{
							ChannelSelector.SelectedIndex = dialog.Selection;
						}
					}
					break;
				default:
					break;
			}

		}

		//TODO: channel copy and paste, focus first menu item on open

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

		private async void CustomMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var item = sender as CustomMenuItem;
			switch (item.Tag)
			{
				case "EQ":
					new EQ4ToolWindow(_channel).ShowDialog();
					break;
				case "Comp":
					new CompToolWindow(_channel).ShowDialog();
					break;
				case "Gate":
					new GateToolWindow(_channel).ShowDialog();
					break;
				case "Limit":
					new LimiterToolWindow(_channel).ShowDialog();
					break;
				case "Routing":
					if (_channel is not StereoLineInput)
						new RoutingToolWindow(_channel).ShowDialog();
					break;
				case "Sends":
					new SendsView(_channel, blindViewModel).ShowDialog();
					break;
				case "Load":
					{
						var win = new FileOpenToolWindow(await blindViewModel.Presets.GetChannelPresets());
						win.ShowDialog();

						if (!win.DialogResult.HasValue || !win.DialogResult.Value) return;
						blindViewModel.Presets.FileOperation(Presets.OperationType.RecallChannel, win.Selection, "", new(_channel));
					}
					break;
				case "FXA":
					new FXComponentWindow(blindViewModel.FX[0], blindViewModel).ShowDialog();
					break;
				case "FXB":
					new FXComponentWindow(blindViewModel.FX[1], blindViewModel).ShowDialog();
					break;
				case "FXC":
					new FXComponentWindow(blindViewModel.FX[2], blindViewModel).ShowDialog();
					break;
				case "FXD":
					new FXComponentWindow(blindViewModel.FX[3], blindViewModel).ShowDialog();
					break;
			}
		}

		private void CustomMenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			new TextPrompt(_channel, _channel.GetType().GetProperty(nameof(_channel.username)), "Rename Channel").ShowDialog();
		}

		private async void LoadProjectMenuButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new FileOpenToolWindow(await blindViewModel.Presets.GetProjects());
			window.ShowDialog();

			if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

			blindViewModel.Presets.FileOperation(Presets.OperationType.RecallProject, window.Selection);
		}

		private async void LoadSceneMenuButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new FileOpenToolWindow(await blindViewModel.Presets.GetScenes());
			window.ShowDialog();

			if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

			blindViewModel.Presets.FileOperation(Presets.OperationType.RecallScene, null, window.Selection);
		}

		private async void SaveProject_Click(object sender, RoutedEventArgs e)
		{
			var window = new FileOpenToolWindow(await blindViewModel.Presets.GetProjects());
			window.ShowDialog();

			if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

			blindViewModel.Presets.FileOperation(Presets.OperationType.StoreProject, window.Selection);
		}

		private async void SaveSceneButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new FileOpenToolWindow(await blindViewModel.Presets.GetScenes());
			window.ShowDialog();

			if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

			blindViewModel.Presets.FileOperation(Presets.OperationType.StoreScene, null, window.Selection);
		}

		private async void SaveSceneAsButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new FileOpenToolWindow(await blindViewModel.Presets.GetScenes());
			window.ShowDialog();

			if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

			var saveFile = window.Selection;
			var segments = saveFile.Split('.');
			var index = segments[0];
			var extension = segments[2];

			var prompt = new TextPrompt("", "Enter a name for the scene file, then press enter.");
			prompt.ShowDialog();

			if (!prompt.DialogResult.HasValue || !prompt.DialogResult.Value) return;

			blindViewModel.Presets.FileOperation(Presets.OperationType.StoreScene, null, $"{index}.{prompt.UserString}.{extension}");
		}

		private async void SaveProjectAs_Click(object sender, RoutedEventArgs e)
		{
			var window = new FileOpenToolWindow(await blindViewModel.Presets.GetProjects());
			window.ShowDialog();

			if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

			var saveFile = window.Selection;
			var segments = saveFile.Split('.');
			var index = segments[0];
			var extension = segments[2];

			var prompt = new TextPrompt("", "Enter a name for the project file, then press enter.");
			prompt.ShowDialog();

			if (!prompt.DialogResult.HasValue || !prompt.DialogResult.Value) return;

			blindViewModel.Presets.FileOperation(Presets.OperationType.StoreProject, null, $"{index}.{prompt.UserString}.{extension}");
		}

		private void resetChannelMenuItem_Click(object sender, RoutedEventArgs e)
		{
			blindViewModel.Presets.ResetChannel(_channel.ChannelType, _channel.ChannelIndex);
		}

		private void copyPasteMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (sender is CustomMenuItem item)
			{
				if ((string)item.Tag == "Copy")
				{
					blindViewModel.Presets.ChannelCopyPaste(new(_channel), false);
				}
				else if ((string)item.Tag == "Paste")
				{
					blindViewModel.Presets.ChannelCopyPaste(new(_channel), true);
				}

			}
		}
	}
}
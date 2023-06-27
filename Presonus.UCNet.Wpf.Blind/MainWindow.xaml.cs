using Microsoft.Extensions.DependencyInjection;
using Presonus.UCNet.Api;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Api.Services;
using Presonus.UCNet.Wpf.Blind.ToolWindows;
using Presonus.UCNet.Wpf.Blind.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModifierKeys = Presonus.UCNet.Wpf.Blind.UserControls.ModifierKeys;

/*
TODO LIST
─────────

[x] make sure keyboard shortcuts for trim and others persist after mix selection is made
[x] fix the issue where aux levels/routing are volatile
[x] compression ratio default value should be 0
[x] keyboard shortcuts, with a focus on the left hand
[ ] introduce aux assigns as mutes
[x] introduce the 'send to main' toggle
[x] scene and project resetting
[x] handle hpf and trim knobs when selecting different channel types (trim knob should be digital gain on stereolineinput?
[x] mute groups
[ ] gate time curves
[ ] multi channel clip protection
*/

namespace Presonus.UCNet.Wpf.Blind
{
    public partial class MainWindow : Window
    {
        private BlindViewModel blindViewModel;
        private Channel _channel
        {
            get;
            set;
        }
        private Channel _mix
        {
            get;
            set;
        }
        private MeterService _meterService;

        private Dictionary<Key, RoutedEventHandler> shortcutActions = new Dictionary<Key, RoutedEventHandler>();
        public float Peak;

        public MainWindow(BlindViewModel viewModel, Speech.SpeechManager speechManager)
        {
            DataContext = blindViewModel = viewModel;
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            Activated += MainWindow_Activated;

            Closed += MainWindow_Closed;
            InitializeShortcuts();
        }

        public float Meter => _channel?.ChannelType != null && _channel.ChannelIndex != null
            ? _meterService.MeterData.GetData(new(_channel.ChannelType, _channel.ChannelIndex - 1))
            : 0.0f;

        void SelectMixWithShortcut(Channel channel)
        {
            foreach (CustomMenuItem item in mixMenu.Items)
            {
                if ((string)item.Header == channel.username) item.IsChecked = true;
                else item.IsChecked = false;
            }
            MixSelected(channel);

            if (channel.link && channel.linkmaster)
                Speech.SpeechManager.Say(channel.username + " stereo pair");
            else
                Speech.SpeechManager.Say(channel.username);
        }

        //HACK: 
        int MaxBank = 1;
        int curBank = 0;

        void HandleFunctionKeys(int number)
        {
            switch (number)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    var auxChannel = blindViewModel.Auxes[(number - 1) + (curBank * 8)];
                    if (auxChannel.linkslave)
                        SelectMixWithShortcut(blindViewModel.Auxes[(number - 2) + (curBank * 8)]);
                    else
                        SelectMixWithShortcut(auxChannel);
                    break;
                case 10: //bank up
                    if (curBank + 1 <= MaxBank)
                    {
                        curBank++;
                    }
                    Speech.SpeechManager.Say("Mixes " + ((curBank * 8) + 1) + " through " + ((curBank * 8) + 8));
                    break;
                case 9:
                    if (curBank - 1 >= 0)
                    {
                        curBank--;
                    }
                    Speech.SpeechManager.Say("Mixes " + ((curBank * 8) + 1) + " through " + ((curBank * 8) + 8));
                    break;
            }
        }

        void HandleNumberKey(int number)
        {
            if (ModifierKeys.IsCtrlDown())
            {
                ToggleMuteGroup(number);
                return;
            }
        }

        void ToggleMuteGroup(int num)
        {
            bool state = false;
            switch (num)
            {
                case 1:
                    state = blindViewModel.Mutegroup.mutegroup1 = !blindViewModel.Mutegroup.mutegroup1;
                    Speech.SpeechManager.Say($"{blindViewModel.Mutegroup.mutegroup1username} " + (state ? "Muted" : "Unmuted"));
                    break;
                case 2:
                    state = blindViewModel.Mutegroup.mutegroup2 = !blindViewModel.Mutegroup.mutegroup2;
                    Speech.SpeechManager.Say($"{blindViewModel.Mutegroup.mutegroup2username} " + (state ? "Muted" : "Unmuted"));
                    break;
                case 3:
                    state = blindViewModel.Mutegroup.mutegroup3 = !blindViewModel.Mutegroup.mutegroup3;
                    Speech.SpeechManager.Say($"{blindViewModel.Mutegroup.mutegroup3username} " + (state ? "Muted" : "Unmuted"));
                    break;
                case 4:
                    state = blindViewModel.Mutegroup.mutegroup4 = !blindViewModel.Mutegroup.mutegroup4;
                    Speech.SpeechManager.Say($"{blindViewModel.Mutegroup.mutegroup4username} " + (state ? "Muted" : "Unmuted"));
                    break;
                case 5:
                    state = blindViewModel.Mutegroup.mutegroup5 = !blindViewModel.Mutegroup.mutegroup5;
                    Speech.SpeechManager.Say($"{blindViewModel.Mutegroup.mutegroup5username} " + (state ? "Muted" : "Unmuted"));
                    break;
                case 6:
                    state = blindViewModel.Mutegroup.mutegroup6 = !blindViewModel.Mutegroup.mutegroup6;
                    Speech.SpeechManager.Say($"{blindViewModel.Mutegroup.mutegroup6username} " + (state ? "Muted" : "Unmuted"));
                    break;
                case 7:
                    state = blindViewModel.Mutegroup.mutegroup7 = !blindViewModel.Mutegroup.mutegroup7;
                    Speech.SpeechManager.Say($"{blindViewModel.Mutegroup.mutegroup7username} " + (state ? "Muted" : "Unmuted"));
                    break;
                case 8:
                    state = blindViewModel.Mutegroup.mutegroup8 = !blindViewModel.Mutegroup.mutegroup8;
                    Speech.SpeechManager.Say($"{blindViewModel.Mutegroup.mutegroup8username} " + (state ? "Muted" : "Unmuted"));
                    break;
            }
        }

        public int IncrementToNextMultipleOfEight(int value)
        {
            int remainder = value % 8;
            if (remainder == 0)
            {
                // 'value' is already a multiple of 8
                return value + 8;
            }
            else
            {
                // Calculate the next multiple of 8
                int nextMultipleOfEight = value + (8 - remainder);
                return nextMultipleOfEight;
            }
        }

        public int DecrementToPreviousMultipleOfEight(int value)
        {
            int remainder = value % 8;
            if (remainder == 0)
            {
                // 'value' is already a multiple of 8
                return value - 8;
            }
            else
            {
                // Calculate the previous multiple of 8
                int previousMultipleOfEight = value - remainder;
                return previousMultipleOfEight;
            }
        }

        private void InitializeShortcuts()
        {
            //MIX SELECTION
            shortcutActions[Key.D0] = (s, e) => HandleNumberKey(0);
            shortcutActions[Key.D1] = (s, e) => HandleNumberKey(1);
            shortcutActions[Key.D2] = (s, e) => HandleNumberKey(2);
            shortcutActions[Key.D3] = (s, e) => HandleNumberKey(3);
            shortcutActions[Key.D4] = (s, e) => HandleNumberKey(4);
            shortcutActions[Key.D5] = (s, e) => HandleNumberKey(5);
            shortcutActions[Key.D6] = (s, e) => HandleNumberKey(6);
            shortcutActions[Key.D7] = (s, e) => HandleNumberKey(7);
            shortcutActions[Key.D8] = (s, e) => HandleNumberKey(8);
            shortcutActions[Key.D9] = (s, e) => HandleNumberKey(9);

            shortcutActions[Key.F1] = (s, e) => HandleFunctionKeys(1);
            shortcutActions[Key.F2] = (s, e) => HandleFunctionKeys(2);
            shortcutActions[Key.F3] = (s, e) => HandleFunctionKeys(3);
            shortcutActions[Key.F4] = (s, e) => HandleFunctionKeys(4);
            shortcutActions[Key.F5] = (s, e) => HandleFunctionKeys(5);
            shortcutActions[Key.F6] = (s, e) => HandleFunctionKeys(6);
            shortcutActions[Key.F7] = (s, e) => HandleFunctionKeys(7);
            shortcutActions[Key.F8] = (s, e) => HandleFunctionKeys(8);
            shortcutActions[Key.F9] = (s, e) => HandleFunctionKeys(9);
            shortcutActions[Key.F10] = (s, e) => HandleFunctionKeys(10);
            shortcutActions[Key.F11] = (s, e) => HandleFunctionKeys(11);
            shortcutActions[Key.F12] = (s, e) => HandleFunctionKeys(12);

            shortcutActions[Key.Escape] = (s, e) => SelectMixWithShortcut(blindViewModel.Main[0]);

            shortcutActions[Key.V] = (s, e) => volumeControl.Focus();
            shortcutActions[Key.F] = (s, e) => hipassControl.Focus();
            shortcutActions[Key.T] = (s, e) => trimControl.Focus();
            shortcutActions[Key.B] = (s, e) => panControl.Focus();
            shortcutActions[Key.G] = (s, e) => new GateToolWindow(_channel).ShowDialog();

            //DEBUG 
            shortcutActions[Key.I] = (s, e) => blindViewModel.Mutegroup.AssignMutesToAGroup(0);

            shortcutActions[Key.O] = (s, e) => blindViewModel.Mutegroup.mutegroup1 = !blindViewModel.Mutegroup.mutegroup1;


            shortcutActions[Key.E] = (s, e) =>
            {
                if (_channel is OutputDACBus)
                    new EQ6ToolWindow(_channel).ShowDialog();
                else
                    new EQ4ToolWindow(_channel).ShowDialog();
            };
            shortcutActions[Key.C] = (s, e) => new CompToolWindow(_channel).ShowDialog();
            shortcutActions[Key.A] = (s, e) => new SendsView(_channel, blindViewModel).ShowDialog();
            shortcutActions[Key.X] = (s, e) =>
            {
                _channel.mute = !_channel.mute;
                if (_channel.mute)
                    Speech.SpeechManager.Say($"Muted");
                else
                    Speech.SpeechManager.Say($"Unmuted");
            };
            shortcutActions[Key.S] = (s, e) =>
            {
                _channel.solo = !_channel.solo;
                if (_channel.solo)
                    Speech.SpeechManager.Say($"Solo On");
                else
                    Speech.SpeechManager.Say($"Solo Off");
            };
            shortcutActions[Key.M] = (s, e) =>
            {
                if (UserControls.ModifierKeys.IsCtrlDown())
                {
                    Speech.SpeechManager.Say($"{ValueTransformer.LinearToVolume((float)Peak)}");
                }

            };
            shortcutActions[Key.Right] = (s, e) =>
            {
                int newIndex = 0;
                if (ModifierKeys.IsCtrlDown())
                    newIndex = Math.Min(IncrementToNextMultipleOfEight(ChannelSelector.SelectedIndex), ChannelSelector.Items.Count - 1);
                else
                    newIndex = Math.Min(ChannelSelector.SelectedIndex + 1, ChannelSelector.Items.Count - 1);
                ChannelSelector.SelectedIndex = newIndex;
                ChannelSelected((Channel)ChannelSelector.SelectedItem);
            };
            shortcutActions[Key.Left] = (s, e) =>
            {
                int newIndex = 0;
                if (ModifierKeys.IsCtrlDown())
                    newIndex = Math.Max(DecrementToPreviousMultipleOfEight(ChannelSelector.SelectedIndex), 0);
                else
                    newIndex = Math.Max(ChannelSelector.SelectedIndex - 1, 0);
                ChannelSelector.SelectedIndex = newIndex;
                ChannelSelected((Channel)ChannelSelector.SelectedItem);
            };
            shortcutActions[Key.F] = (s, e) => //CTRL+F channel finder
            {
                if (ModifierKeys.IsCtrlDown())
                {
                    var dialog = new ChannelSelectorToolWindow(blindViewModel);
                    dialog.ShowDialog();

                    if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                    {
                        ChannelSelector.SelectedIndex = dialog.Selection;
                        ChannelSelected((Channel)ChannelSelector.SelectedItem);
                    }
                }
            };
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void BuildControls()
        {
            switch (_mix.ChannelType)
            {
                case ChannelTypes.MAIN:
                    {
                        mixPanel.Children.Clear();
                        //< usercontrols:NumericUpDown x:Name = "trimControl" Caption = "Trim" Value = "{Binding preampgain}" Curve = "Linear" Min = "0" Max = "60" Unit = "DB" Default = "0" />
                        trimControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Trim", 0, 60, 0, Units.DB, CurveFormula.Linear, "trim");

                        //< usercontrols:NumericUpDown x:Name = "volumeControl" Caption = "Level" Value = "{Binding volume}" Curve = "LinearToVolume" Min = "-84" Max = "10" Unit = "DB" Default = "0.735" />
                        volumeControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Level", -84, 10, 0.735f, Units.DB, CurveFormula.LinearToVolume, "volume");

                        //< usercontrols:NumericUpDown x:Name = "hipassControl" Caption = "Hi Pass Filter" Value = "{Binding hpf}" Curve = "Logarithmic" Min = "24" Max = "1000" Unit = "HZ_24OFF" Default = "0" />
                        hipassControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Hi Pass Filter", 24, 1000, 0, Units.HZ_24OFF, CurveFormula.Logarithmic, "hpf");

                        if (_channel.link == true || _channel is StereoLineInput)
                            panControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Stereo Width", 0, 100, 1f, Units.PERCENT, CurveFormula.Linear, "stereopan");
                        else
                            panControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Pan", 0, 1, 0.5f, Units.PAN, CurveFormula.Linear, "pan");
                        //< usercontrols:NumericUpDown x:Name = "panControl" Caption = "Pan" Value = "{Binding pan}" Curve = "Linear" Min = "0" Max = "1" Unit = "PAN" Default = "0.5" />
                    }
                    break;

                case ChannelTypes.AUX:
                    {
                        var chanNum = _mix.ChannelIndex;
                        mixPanel.Children.Clear();

                        //< usercontrols:NumericUpDown x:Name = "trimControl" Caption = "Trim" Value = "{Binding preampgain}" Curve = "Linear" Min = "0" Max = "60" Unit = "DB" Default = "0" />
                        trimControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Trim", 0, 60, 0, Units.DB, CurveFormula.Linear, "trim");

                        //< usercontrols:NumericUpDown x:Name = "volumeControl" Caption = "Level" Value = "{Binding volume}" Curve = "LinearToVolume" Min = "-84" Max = "10" Unit = "DB" Default = "0.735" />
                        volumeControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Send Level", -84, 10, 0.735f, Units.DB, CurveFormula.LinearToVolume, $"aux{chanNum}");

                        //< usercontrols:NumericUpDown x:Name = "hipassControl" Caption = "Hi Pass Filter" Value = "{Binding hpf}" Curve = "Logarithmic" Min = "24" Max = "1000" Unit = "HZ_24OFF" Default = "0" />
                        hipassControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Hi Pass Filter", 24, 1000, 0, Units.HZ_24OFF, CurveFormula.Logarithmic, "hpf");

                        var auxChan = blindViewModel.Auxes[chanNum - 1];
                        if (auxChan.linkmaster)
                        {
                            if (_channel.link == true || _channel is StereoLineInput)
                                panControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Stereo Width", 0, 100, 1, Units.PERCENT, CurveFormula.Linear, $"aux{chanNum}{chanNum + 1}_stpan");
                            else
                                panControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Pan", 0, 1, 0.5f, Units.PAN, CurveFormula.Linear, $"aux{chanNum}{chanNum + 1}_pan");
                        }
                        else if (auxChan.link && !auxChan.linkmaster)
                        {
                            if (_channel.link == true || _channel is StereoLineInput)
                                panControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Stereo Width", 0, 100, 1, Units.PAN, CurveFormula.Linear, $"aux{chanNum - 1}{chanNum}_stpan");
                            else
                                panControl = ControlFactory.CreateNumericUpDownControl(mixPanel, "Pan", 0, 1, 0.5f, Units.PAN, CurveFormula.Linear, $"aux{chanNum - 1}{chanNum}_pan");
                        }
                    }
                    break;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _meterService = App.ServiceProvider.GetRequiredService<MeterService>();
            _meterService.MeterDataReceived += _meterService_MeterDataReceived;

            var mainChannel = blindViewModel.Main[0];
            _mix = mainChannel;
            ChannelSelector.SelectedIndex = 0;
            _channel = (Channel)ChannelSelector.SelectedItem;
            BuildControls();
            BuildMixMenu(mainChannel);
            BuilGEQMenu();
            BuildFXMenu();
            trimControl.Focus();
        }

        private void BuildFXMenu()
        {
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

                var menuItem = new CustomMenuItem();
                menuItem.Header = $"FX{letter}";
                menuItem.Tag = $"FX{letter}";
                menuItem.Click += CustomMenuItem_Click;
                viewMenu.Items.Add(menuItem);
            }
        }

        private void BuilGEQMenu()
        {
            for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.GEQ]; i++)
            {
                int index = i;
                if (index < 6)
                {
                    var linked = blindViewModel.Auxes[i].linkmaster;
                    var menuItem = new CustomMenuItem()
                    {
                        Header = $"Graphic EQ ({blindViewModel.Auxes[i].username})" + (linked ? " (linked pair)" : "")
                    };
                    if (linked)
                    {
                        i++;
                    }
                    menuItem.Click += (s, e) =>
                    {
                        new GEQWindow(blindViewModel.GEQ[index], blindViewModel).ShowDialog();
                    };
                    viewMenu.Items.Add(menuItem);
                }
                else
                {
                    var menuItem = new CustomMenuItem()
                    {
                        Header = $"Graphic EQ ({blindViewModel.Main[0].username})"
                    };
                    i++;
                    menuItem.Click += (s, e) =>
                    {
                        new GEQWindow(blindViewModel.GEQ[index], blindViewModel).ShowDialog();
                    };
                    viewMenu.Items.Add(menuItem);
                }
            }
        }

        Dictionary<int, Key> numberKeys = new Dictionary<int, Key>() {
            { 0, Key.D0 },
            { 1, Key.D1 },
            { 2, Key.D2 },
            { 3, Key.D3 },
            { 4, Key.D4 },
            { 5, Key.D5 },
            { 6, Key.D6 },
            { 7, Key.D7 },
            { 8, Key.D8 },
        };


        private void BuildMixMenu(OutputDACBus mainChannel)
        {
            for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.AUX]; i++)
            {
                var auxChannel = blindViewModel.Auxes[i];
                var menuItem1 = new CustomMenuItem()
                {
                    Header = auxChannel.username
                };
                menuItem1.IsCheckable = true;
                menuItem1.Click += (s, e) =>
                {
                    foreach (var item in mixMenu.Items) ((CustomMenuItem)item).IsChecked = false;
                    menuItem1.IsChecked = true;
                    MixSelected(auxChannel);
                };

                mixMenu.Items.Add(menuItem1);
            }

            var menuItem = new CustomMenuItem()
            {
                Header = mainChannel.username
            };
            menuItem.IsCheckable = true;
            menuItem.Click += (s, e) =>
            {
                foreach (var item in mixMenu.Items) ((CustomMenuItem)item).IsChecked = false;
                menuItem.IsChecked = true;
                MixSelected(mainChannel);

            };
            menuItem.IsChecked = true;

            mixMenu.Items.Add(menuItem);

        }

        private void MainWindow_Activated(object? sender, EventArgs e)
        {
            Speech.SpeechManager.Say($"{Title} ");
            if (_channel != null)
                Speech.SpeechManager.Say($"Channel {_channel.chnum} selected", false);
        }

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
                        input.trim -= 0.01f;
                    }
                }
            }
        }

        private void ChannelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChannelSelector.SelectedItem is Channel channel)
            {
                ChannelSelected(channel);
            }
        }

        private void ReportChannel()
        {
            _channel.select = true;
            Speech.SpeechManager.Say($"{_channel.chnum} ({_channel.username})");
            if (_channel.link)
            {
                if (_channel.linkmaster)
                {
                    Speech.SpeechManager.Say($"Left side of linked pair", false);
                }
                else
                {
                    Speech.SpeechManager.Say($"Right side of linked pair", false);
                }
            }

            // Check if the focused element is an IAccessibleControl
            if (Keyboard.FocusedElement is NumericUpDown focusedControl)
            {
                string valueString = focusedControl.ValueString;
                Speech.SpeechManager.Say(valueString, false);
            }
        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System)
            {
                if (shortcutActions.ContainsKey(e.SystemKey))
                {
                    shortcutActions[e.SystemKey]?.Invoke(null, null);
                    e.Handled = true;
                }
            }
            if (Keyboard.FocusedElement is CustomMenuItem)
            {
                return;
            }

            if (shortcutActions.ContainsKey(e.Key))
            {
                e.Handled = true;
                shortcutActions[e.Key]?.Invoke(null, null);
            }
        }

        //TODO: channel copy and paste, focus first menu item on open

        private void ValidateControls()
        {
            if (_channel is OutputDACBus && _mix.ChannelType == ChannelTypes.AUX)
            {
                volumeControl.Visibility = Visibility.Collapsed;
            }
            else volumeControl.Visibility = Visibility.Visible;

            if (_channel is OutputDACBus)
            {
                trimControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                trimControl.Visibility = Visibility.Visible;
                if (_channel is MicLineInput input)
                {
                    if (input.preampmode == 0) //gain
                    {
                        trimControl.Min = 0;
                        trimControl.Max = 60;

                        trimControl.Default = 0;
                    }
                    else //preamp
                    {
                        trimControl.Min = 0;
                        trimControl.Max = 20;

                        trimControl.Default = 0.5f;

                    }
                    var expr = trimControl.GetBindingExpression(NumericUpDown.ValueProperty);
                    expr.UpdateSource();
                }
            }

            if (_channel is StereoLineInput)
            {
                hipassControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                hipassControl.Visibility = Visibility.Visible;
            }


            if (_channel is MicLineInput micinput)
            {
                preampMenuItem.Visibility = Visibility.Visible;

                bool preampActive = micinput.preampmode == 0;
                if (preampActive)
                    preampMenuItem.Header = "Preamp Active";
                else
                    preampMenuItem.Header = "Preamp Inactive";

            }
            else
            {
                preampMenuItem.Visibility = Visibility.Collapsed;
            }
        }

        private void ChannelSelected(Channel channel)
        {
            _channel = channel;
            BuildControls();
            ValidateControls();
            ReportChannel();

        }

        private void MixSelected(Channel mix)
        {
            _mix = mix;
            BuildControls();
            ValidateControls();
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
                    //if (_channel is not StereoLineInput)
                    new RoutingToolWindow(_channel).ShowDialog();
                    break;

                case "Sends":
                    new SendsView(_channel, blindViewModel).ShowDialog();
                    break;

                case "Load":
                    {
                        var win = new FileOpenToolWindow(await blindViewModel.Presets.GetChannelPresets(), "Load Channel");
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

                case "SignalGen":
                    new SignalGenToolWindow(blindViewModel.SignalGen).ShowDialog();
                    break;
            }
        }

        private void CustomMenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            new TextPrompt(_channel, _channel.GetType().GetProperty(nameof(_channel.username)), "Rename Channel").ShowDialog();
        }

        private async void LoadProjectMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new FileOpenToolWindow(await blindViewModel.Presets.GetProjects(), "Load Project");
            window.ShowDialog();

            if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

            blindViewModel.Presets.FileOperation(Presets.OperationType.RecallProject, window.Selection);
        }

        private async void LoadSceneMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new FileOpenToolWindow(await blindViewModel.Presets.GetScenes(), "Load Scene");
            window.ShowDialog();

            if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

            blindViewModel.Presets.FileOperation(Presets.OperationType.RecallScene, blindViewModel.Presets.LoadedProjectName, window.Selection);
        }

        private async void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            var window = new FileOpenToolWindow(await blindViewModel.Presets.GetProjects(), "Save Project");
            window.ShowDialog();

            if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

            blindViewModel.Presets.FileOperation(Presets.OperationType.StoreProject, window.Selection);
        }

        private async void SaveSceneButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new FileOpenToolWindow(await blindViewModel.Presets.GetScenes(), "Save Scene");
            window.ShowDialog();

            if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

            blindViewModel.Presets.FileOperation(Presets.OperationType.StoreScene, null, window.Selection);
        }

        private async void SaveSceneAsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new FileOpenToolWindow(await blindViewModel.Presets.GetScenes(), "Save Scene As");
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
            var window = new FileOpenToolWindow(await blindViewModel.Presets.GetProjects(), "Save Project As");
            window.ShowDialog();

            if (!window.DialogResult.HasValue || !window.DialogResult.Value) return;

            var saveFile = window.Selection;
            var segments = saveFile.Split('.');
            var index = segments[0];
            var extension = segments[2];

            var prompt = new TextPrompt("", "Enter a name for the project file, then press enter.");
            prompt.ShowDialog();

            if (!prompt.DialogResult.HasValue || !prompt.DialogResult.Value) return;

            blindViewModel.Presets.FileOperation(Presets.OperationType.StoreProject, $"{index}.{prompt.UserString}.{extension}");
        }

        private void resetChannelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            blindViewModel.Presets.ResetChannel(_channel.ChannelType, _channel.ChannelIndex);
        }

        private void assignMutes(object sender, RoutedEventArgs e)
        {
            if (sender is CustomMenuItem item)
            {
                var tag = int.Parse((string)item.Tag);
                blindViewModel.Mutegroup.AssignMutesToAGroup(tag - 1);

            }
        }

        private void renameMuteGroup(object sender, RoutedEventArgs e)
        {

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

        private void projectFilterMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CustomMenuItem item)
                new ProjectSceneFiltersToolWindow(blindViewModel, (string)item.Tag).ShowDialog();
        }

        private void settingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingsToolWindow(blindViewModel).ShowDialog();
        }

        private void resetProject_Click(object sender, RoutedEventArgs e)
        {
            blindViewModel.Presets.FileOperation(Presets.OperationType.ResetProject);
        }

        private void resetScene_Click(object sender, RoutedEventArgs e)
        {
            blindViewModel.Presets.FileOperation(Presets.OperationType.ResetScene);
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
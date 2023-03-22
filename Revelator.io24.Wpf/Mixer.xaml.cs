using Microsoft.Win32;
using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Inputs;
using Presonus.StudioLive32.Wpf.UserControls;
using Presonus.StudioLive32.Wpf.Views;
using Presonus.UCNet.Api;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Presonus.StudioLive32.Wpf
{

    public partial class Mixer : Window
    {
        private MainViewModel vm;
        public Mixer(MainViewModel viewModel)
        {
            DataContext = vm = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            InitializeComponent();
            ScreenReaderText = screenReaderText;
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //RawServiceNew.Instance.JSON();
        //    while (!RawService.ConnectionEstablished)
        //    {
        //        await Task.Delay(100);
        //    }

        //    var task = Task.Run(() => LoadLastUsedScene());
        //    var dialog = new LoadingDialog();

        //    task.ContinueWith(t => Application.Current.Dispatcher.Invoke(() => { dialog.Close(); DeviceRoutingBase.loadingFromScene = false; }));
        //    dialog.ShowDialog();
        //    timer = new() { Interval = TimeSpan.FromSeconds(2) };
        //    timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (isReadingMeter)
                ReportValueOfControl(meter);
        }


        public void LoadLastUsedScene()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PreSonus\\Mixer\\current.scn";
            //RawServiceNew.Instance.TryLoadScene(path);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider && slider.IsFocused)
            {
                //Console.WriteLine("SLIDER VALUE CHANGE: {0} - {1} ", e.OldValue, e.NewValue);

                ReportValueOfControl(slider);
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
        }

        public static AutomationPeer AutomationPeer;
        public static TextBlock ScreenReaderText;

        public static void ReadTextToScreenReader(string text)
        {
            //if (oldText == text) return;
            ScreenReaderText.Text = text;
            AutomationPeer = UIElementAutomationPeer.FromElement(ScreenReaderText);
            if (AutomationPeer != null)
            {
                AutomationPeer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
                Console.WriteLine("SCREENREADER: " + text);
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PreSonus\\Mixer\\current.scn";
            //RawServiceNew.Instance.TrySaveScene(path);
            base.OnClosed(e);
            Application.Current.Shutdown();
#if DEBUG
            Environment.Exit(0);
#endif
        }

        public static void ReportValueOfControl(Control control, bool includeNameFirst = false)
        {
            if(control is ProgressBar progressBar)
            {
                ReadTextToScreenReader(Math.Round(progressBar.Value, 2) + " db");
            }
            if (control is Slider slider)
            {
                BindingExpression be = BindingOperations.GetBindingExpression(slider, Slider.ValueProperty);
                string Name = slider.Name;
                if (Name == null || Name == string.Empty)
                {
                    Name = AutomationProperties.GetName(slider);
                }
                if (includeNameFirst)
                {
                    ReadTextToScreenReader(Name + " " + Math.Round(slider.Value, 2) + slider.Tag?.ToString());
                }
                else
                {
                    ReadTextToScreenReader(Math.Round(slider.Value, 2) + slider.Tag?.ToString());
                }
            }
            if (control is Fader fader)
            {
                BindingExpression be = BindingOperations.GetBindingExpression(fader, Fader.ValueProperty);
                string Name = fader.Caption;
                if (Name == null || Name == string.Empty)
                {
                    Name = AutomationProperties.GetName(fader);
                }
                if (includeNameFirst)
                {
                    ReadTextToScreenReader(Name + " " + Math.Round(fader.Value, 2) + " " + fader.Unit?.ToString());
                }
                else
                {
                    ReadTextToScreenReader(Math.Round(fader.Value, 2) + " " + fader.Unit?.ToString());
                }
            }
            if (control is PanPot panpot)
            {
                BindingExpression be = BindingOperations.GetBindingExpression(panpot, PanPot.ValueProperty);
                string Name = panpot.Name;
                if (Name == null || Name == string.Empty)
                {
                    Name = AutomationProperties.GetName(panpot);
                }
                if (includeNameFirst)
                {
                    ReadTextToScreenReader(Name + " " + Math.Round(panpot.Value, 2) + " " + panpot.Unit?.ToString());
                }
                else
                {
                    ReadTextToScreenReader(Math.Round(panpot.Value, 2) + " " + panpot.Unit?.ToString());
                }
            }
        }
        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UIElementAutomationPeer.CreatePeerForElement(screenReaderText);
            foreach (var ctrl in this.GetChildren())
            {
                if (ctrl is Fader fader)
                {
                    UIElementAutomationPeer.CreatePeerForElement(fader);
                    fader.Focused += Fader_Focused1;
                    fader.TextChanged += Fader_TextChanged1;
                }

                if (ctrl is Slider slider)
                {
                    UIElementAutomationPeer.CreatePeerForElement(slider);
                }

                if (ctrl is CheckBox checkBox)
                {
                    UIElementAutomationPeer.CreatePeerForElement(checkBox);
                }
            }

            ChannelList.SelectedIndex = 0;
            ChannelList.Focus();
        }

        private void Fader_TextChanged1(object sender, RoutedEventArgs e)
        {
            if (sender is Fader fader)
            {
                ReportValueOfControl(fader, true);
            }
        }

        private void Fader_Focused1(object sender, RoutedEventArgs e)
        {
            if (sender is Fader fader)
            {
                ReportValueOfControl(fader, true);
            }
        }

        private void Slider_GotFocus(object sender, RoutedEventArgs e)
        {
            ReportValueOfControl((Slider)sender, true);
        }

        private void ClipChecked(object sender, RoutedEventArgs e)
        {
            if (e.Source is CheckBox checkbox)
            {

            }
        }

        private void ChannelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is ListBox listBox)
            {
                if (listBox.SelectedItem is ChannelBase channel)
                {
                   // vm.SelectedChannel = channel;
                }
            }
        }

        private void saveScene(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Custom Scene File (.scn) | *.scn";
            saveFileDialog.DefaultExt = ".scn";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.InitialDirectory = "C:\\Dev\\Scenes\\";
            saveFileDialog.Title = "Save Scene File...";
            var res = saveFileDialog.ShowDialog();
            if (res.HasValue && res.Value == true)
            {
                //RawServiceNew.Instance.TrySaveScene(saveFileDialog.FileName);
            }
        }

        private void loadScene(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Custom Scene File (.scn) | *.scn";
            openFileDialog.DefaultExt = ".scn";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = "C:\\Dev\\Scenes\\";
            openFileDialog.Title = "Load Scene File...";
            var res = openFileDialog.ShowDialog();
            if (res.Value == false || res.HasValue == false)
            {
                return;
            }

        }

        private void Slider_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is Slider slider)
            {
                if (e.Key == Key.Delete)
                {
                    slider.Value = GetDefaultValueProperty(slider);
                }
                if (e.Key == Key.PageUp)
                {
                    slider.Value *= 1.1f;
                    e.Handled = true;
                }
                if (e.Key == Key.PageDown)
                {
                    slider.Value *= 0.9;
                    e.Handled = true;
                }
            }

        }
        private void sendsOnFadersPanelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChannelList.SelectedItem is Api.Models.Auxes.BusChannel channel)
            {
                //new SendsOnFadersPanel(channel, vm.Device).ShowDialog();
            }
            if (ChannelList.SelectedItem is InputChannel inputChannel)
            {
                new AuxSendsView(inputChannel).ShowDialog();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.OemPlus)
            //{
            //    int next = ChannelList.Items.IndexOf(ChannelList.SelectedItem) + 1;
            //    if (next > ChannelList.Items.Count - 1)
            //    {
            //        return;
            //    }

            //    ChannelList.SelectedIndex = next;
            //    var chan = ChannelList.SelectedItem as ChannelBase;
            //    ReadTextToScreenReader(chan.username);
            //}
            //if (e.Key == Key.OemMinus)
            //{
            //    int prev = ChannelList.Items.IndexOf(ChannelList.SelectedItem) - 1;
            //    if (prev < 0)
            //    {
            //        return;
            //    }

            //    ChannelList.SelectedIndex = prev;
            //    var chan = ChannelList.SelectedItem as ChannelBase;
            //    ReadTextToScreenReader(chan.username);
            //}
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.D1)
                {
                    MainTabs.SelectedIndex = 0;
                }
                if (e.Key == Key.D2)
                {
                    MainTabs.SelectedIndex = 1;
                }
                if (e.Key == Key.D3)
                {
                    MainTabs.SelectedIndex = 2;
                }
                if (e.Key == Key.D4)
                {
                    MainTabs.SelectedIndex = 3;
                }
                if (e.Key == Key.D5)
                {
                    MainTabs.SelectedIndex = 4;
                }
                if (e.Key == Key.D6)
                {
                    MainTabs.SelectedIndex = 5;
                }
                if (e.Key == Key.T)
                {
                    Trim.Focus();
                }
                if (e.Key == Key.C)
                {
                    ChannelList.Focus();
                }

                if (e.Key == Key.L || e.Key == Key.V)
                {
                    Level.Focus();
                }

                if (e.Key == Key.H)
                {
                    MainTabs.SelectedIndex = 0;
                    hiPassSlider.Focus();
                }
            }
        }
        public static float GetDefaultValueProperty(DependencyObject obj)
        {
            return (float)obj.GetValue(DefaultValueProperty);
        }

        public static void SetDefaultValueProperty(DependencyObject obj, float value)
        {
            obj.SetValue(DefaultValueProperty, value);
        }

        // Using a DependencyPropertyExample as the backing store for MyProperty.  This enables animation, styling, binding, etc...  
        public static readonly DependencyProperty DefaultValueProperty =
            DependencyProperty.RegisterAttached("DefaultValue", typeof(float), typeof(Mixer), new PropertyMetadata((float)0));


        private void resetEQ_Click(object sender, RoutedEventArgs e)
        {
            //if (vm.SelectedChannel is InputChannel chan)
            //{
            //    chan.eq_bandon1 = true;
            //    chan.eq_bandon2 = true;
            //    chan.eq_bandon3 = true;
            //    chan.eq_freq1 = 80;
            //    chan.eq_freq2 = 400;
            //    chan.eq_freq3 = 2000;
            //    chan.eq_freq4 = 8000;
            //    chan.eq_gain1 = 0;
            //    chan.eq_gain2 = 0;
            //    chan.eq_gain3 = 0;
            //    chan.eq_gain4 = 0;
            //    chan.eq_q1 = 2;
            //    chan.eq_q2 = 2;
            //    chan.eq_q3 = 2;
            //    chan.eq_q4 = 2;
            //}
        }

        private void Fader_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Fader fader)
            {
                ReportValueOfControl(fader);
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Fader_Focused(object sender, RoutedEventArgs e)
        {
            if (sender is Fader fader)
            {
                ReportValueOfControl(fader, true);
            }
        }

        private void Fader_ValueChanged_1(object sender, RoutedEventArgs e)
        {

        }

        private void Fader_TextChanged(object sender, RoutedEventArgs e)
        {

        }
        bool isReadingMeter = false;
        private DispatcherTimer timer;

        private void meter_GotFocus(object sender, RoutedEventArgs e)
        {
            ReadTextToScreenReader("Channel meter");
            isReadingMeter = true;
            timer.Start();

        }

        private void meter_LostFocus(object sender, RoutedEventArgs e)
        {
            isReadingMeter = false;
            timer.Stop();

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }



    public class MyList : ListView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            FrameworkElement source = element as FrameworkElement;

            source.SetBinding(AutomationProperties.AutomationIdProperty, new Binding
            {
                Path = new PropertyPath("Content.AutomationId"),
                RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self }
            });

            source.SetBinding(AutomationProperties.NameProperty, new Binding
            {
                Path = new PropertyPath("Content.AutomationName"),
                RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self }
            });
        }
    }
}



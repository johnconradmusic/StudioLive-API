using Revelator.io24.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Revelator.io24.Wpf
{
    /// <summary>
    /// Interaction logic for Mixer.xaml
    /// </summary>
    public partial class Mixer : Window
    {
        MainViewModel vm;
        public Mixer(MainViewModel viewModel)
        {
            DataContext = vm = viewModel;

            InitializeComponent();
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.Source is Slider slider)
            {
                if (slider.Tag != null && slider.Tag.ToString() == "hz" && slider.Value > 1000)
                {
                    var khzVal = slider.Value / 1000;
                    ReadTextToScreenReader(khzVal.ToString("F1") + " " + "khz");

                }
                if (slider.Tag != null && slider.Tag.ToString() == "pan")
                {
                    var valString = "";
                    switch ((int)slider.Value)
                    {
                        case 0:
                            valString = "center";
                            break;
                        case > 0:
                            valString = Math.Abs(slider.Value).ToString("F0") + " percent right";
                            break;
                        case < 0:
                            valString = Math.Abs(slider.Value).ToString("F0") + " percent left";
                            break;
                    }
                    ReadTextToScreenReader(valString);

                }
                else
                    ReadTextToScreenReader(slider.Value.ToString("F2") + " " + slider.Tag);
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public void ReadTextToScreenReader(string text)
        {
            this.screenReaderText.Text = text;
            var peer = UIElementAutomationPeer.FromElement(screenReaderText);
            if (peer != null)
            {

                peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
#if DEBUG
            Environment.Exit(0);
#endif
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            //var vm = DataContext as MainViewModel;
            //vm.Device.RawService.JSON();
            //ChannelList.Items.Refresh();
        }

        private void testButton_Click_1(object sender, RoutedEventArgs e)
        {
            //var vm = DataContext as MainViewModel;
            //vm.Device.RawService.SetValue("line/ch1/preampgain", .5f);
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FocusManager.GetFocusedElement(this) is Slider slider)
            {
                var peer = UIElementAutomationPeer.FromElement(slider);
                if (peer == null) return;
                if (slider.Tag != null && slider.Tag.ToString() == "hz" && slider.Value > 1000)
                {
                    var khzVal = slider.Value / 1000;
                    ReadTextToScreenReader(peer.GetName() + " " + khzVal.ToString("F1") + " " + "khz");

                }
                if (slider.Tag != null && slider.Tag.ToString() == "pan")
                {
                    var valString = "";
                    switch ((int)slider.Value)
                    {
                        case 0:
                            valString = "center";
                            break;
                        case > 0:
                            valString = Math.Abs(slider.Value).ToString("F0") + " percent right";
                            break;
                        case < 0:
                            valString = Math.Abs(slider.Value).ToString("F0") + " percent left";
                            break;
                    }
                    ReadTextToScreenReader(UIElementAutomationPeer.FromElement(slider).GetName() + " " + valString);

                }
                else
                    ReadTextToScreenReader(UIElementAutomationPeer.FromElement(slider).GetName() + " " + slider.Value.ToString("F2") + " " + slider.Tag);
            }
            if (FocusManager.GetFocusedElement(this) is CheckBox checkbox)
            {
                if (checkbox.IsChecked.HasValue)
                {
                    var peer = UIElementAutomationPeer.FromElement(checkbox);
                    if (peer == null) return;
                    ReadTextToScreenReader(peer.GetName() + " " + (checkbox.IsChecked.Value ? "On" : "Off"));
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                var stackPanel = new StackPanel();
                var slider = new Slider();

                Binding valueBinding = new Binding();
                //valueBinding.Source = vm;
                valueBinding.ElementName = "ChannelList";
                valueBinding.Path = new PropertyPath("SelectedItem.aux" + (i + 1).ToString());
                valueBinding.Mode = BindingMode.TwoWay;
                slider.SetBinding(Slider.ValueProperty, valueBinding);
                slider.Width = 40;
                slider.Height = 100;
                slider.Tag = "db";
                slider.Minimum = -84;
                slider.Maximum = 10;
                slider.Orientation = Orientation.Vertical;

                Binding nameBinding = new Binding();
                nameBinding.Path = new PropertyPath("Device.Buses[" + i.ToString() + "].username");
                //nameBinding.Source = vm;
                slider.SetBinding(Slider.NameProperty, nameBinding);

                var sliderPeer = UIElementAutomationPeer.CreatePeerForElement(slider);

                stackPanel.Children.Add(slider);

                TextBlock textBlock = new TextBlock();
                textBlock.SetBinding(TextBlock.TextProperty, nameBinding);

                stackPanel.Children.Add(textBlock);
                ChannelSendsToAuxes.Children.Add(stackPanel);
            }
            //        < StackPanel >
            //< Slider IsEnabled = "{Binding Device.Buses[0].LinkSlave}" Value = "{Binding ElementName=ChannelList, Path=SelectedItem.aux1, Mode=TwoWay, FallbackValue=0}" AutomationProperties.Name = "{Binding Device.Channels[0].username}" Tag = "db" AutomationProperties.LiveSetting = "Off"  SmallChange = "1" HorizontalAlignment = "Center" Height = "100" Orientation = "Vertical" Minimum = "-84"  Maximum = "10" TickPlacement = "Both" Ticks = "0.0" />

            //                         < TextBlock Width = "40" Margin = "5" VerticalAlignment = "Center" Text = "{Binding Device.Buses[0].username}" />

            //                            </ StackPanel >

            UIElementAutomationPeer.CreatePeerForElement(screenReaderText);
            foreach (var ctrl in this.GetChildren())
            {
                if (ctrl is Slider slider)
                {
                    UIElementAutomationPeer.CreatePeerForElement(slider);
                    slider.ValueChanged += Slider_ValueChanged;
                }
                if (ctrl is CheckBox checkBox)
                    UIElementAutomationPeer.CreatePeerForElement(checkBox);
            }
            vm.Device.RawService.JSON();
            ChannelList.Items.Refresh();
            ChannelList.SelectedIndex = 0;
            ChannelList.Focus();
        }

        private void ChannelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChannelList.SelectedItem is ChannelBase channel)
            {
                vm.SelectedChannel = channel;
            }
            else vm.SelectedChannel = null;
            vm.OnPropertyChanged(nameof(vm.SelectedChannelIsBus));
        }
    }

    public class MySlider : Slider
    {
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            UIElementAutomationPeer.FromElement(this).InvalidatePeer();
            base.OnValueChanged(oldValue, newValue);
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

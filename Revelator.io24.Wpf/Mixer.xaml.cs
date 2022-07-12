using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Monitor;
using Presonus.StudioLive32.Wpf.Views;
using Presonus.UC.Api.Sound;
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
using System.Windows.Threading;

namespace Presonus.StudioLive32.Wpf
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
            vm.Device.RawService.JSON();

            InitializeComponent();

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
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
        public void ReadTextToScreenReader(string text)
        {
            //if (oldText == text) return;
            this.screenReaderText.Text = text;
            var peer = UIElementAutomationPeer.FromElement(screenReaderText);
            if (peer != null)
            {
                peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
                //Console.WriteLine("SCREENREADER: " + text);
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

        public void ReportValueOfControl(Slider control, bool includeNameFirst = false)
        {

            if (control is Slider slider)
            {
                BindingExpression be = BindingOperations.GetBindingExpression(slider, (Slider.ValueProperty));
                string Name = slider.Name;
                if (includeNameFirst)
                    ReadTextToScreenReader(Name + " " + Math.Round(slider.Value, 2) + slider.Tag?.ToString());
                else
                    ReadTextToScreenReader(Math.Round(slider.Value, 2) + slider.Tag?.ToString());
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
                if (ctrl is Slider slider)
                    UIElementAutomationPeer.CreatePeerForElement(slider);

                if (ctrl is CheckBox checkBox)
                    UIElementAutomationPeer.CreatePeerForElement(checkBox);
            }

            ChannelList.SelectedIndex = 0;
            ChannelList.Focus();
        }

        private void eqPanelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChannelList.SelectedItem is ChannelBase channel)
            {
                new EQ_Panel(channel).ShowDialog();
            }
        }

        private void channelToolsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChannelList.SelectedItem is ChannelBase channel)
            {
                new ChannelTools(channel).ShowDialog();
            }
            ChannelList.Items.Refresh();
        }

        private void auxPanelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChannelList.SelectedItem is ChannelBase channel)
            {
                new AuxSendsView(channel).ShowDialog();
            }
        }

        private void compPanelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChannelList.SelectedItem is ChannelBase channel)
            {
                new DynamicsPanel(channel).ShowDialog();
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
                    vm.SelectedChannel = channel;
                }
            }
        }

        private void saveScene(object sender, RoutedEventArgs e)
        {
            Presonus.UC.Api.Services.Serializer.Serialize(vm.Device);
        }

        private void loadScene(object sender, RoutedEventArgs e)
        {
            string jsonString = UC.Api.Services.Serializer.Deserialize("C:\\Dev\\scenefile.scene");
            vm.Device.SetStateFromLoadedSceneFile(jsonString);
        }

        private void recallScene_Click(object sender, RoutedEventArgs e)
        {
            //vm.Device.RawService.SetValue("presets/scn", 0);
            for (float i = 0; i < 1; i += 0.1f)
            {
                Console.WriteLine(i + " - DB from float - " + UC.Api.Helpers.Util.GetDBFromFloat(i));
            }
            for (int i = -84; i < 10; i+=5)
            {
                Console.WriteLine(i + " - float from DB - " + UC.Api.Helpers.Util.GetFloatFromDB(i));
            }
        }

        private void Slider_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(sender is Slider slider)
            {
                if(e.Key == Key.Delete)
                {
                    slider.Value = 0;
                }
            }
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

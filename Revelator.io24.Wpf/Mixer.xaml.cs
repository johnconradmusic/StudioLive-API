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

       

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)        {

            Serilog.Log.Information("slider value changed");

            if (e.Source is Slider slider)
            {
                var min = slider.Minimum;
                var max = slider.Maximum;

                ReadTextToScreenReader("VALUE CHANGED");
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public void ReadTextToScreenReader(string text)
        {
            var peer = UIElementAutomationPeer.FromElement(screenReaderText);
            if (peer != null)
            {
                this.screenReaderText.Text = text;
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
            Serilog.Log.Information("FOCUS CHANGE");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var ctrl in this.GetChildren())
            {
                if (ctrl is Slider slider)
                {
                    Serilog.Log.Information("its a slider");
                    slider.ValueChanged += Slider_ValueChanged;
                    //if (vm.Device.RawService._valueRanges.TryGetValue(slider.Tag.ToString(), out Tuple<float, float> range))
                    //{
                    //    slider.Minimum = range.Item1;
                    //    slider.Maximum = range.Item2;
                    //}
                }
            }
            var vm = DataContext as MainViewModel;
            vm.Device.RawService.JSON();
            ChannelList.Items.Refresh();
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

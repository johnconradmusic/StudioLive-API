using Presonus.StudioLive32.Api.Models;
using Presonus.UC.Api;
using Presonus.UC.Api.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Presonus.StudioLive32.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ChannelTools.xaml
    /// </summary>
    public partial class ChannelTools : Window
    {
        public ChannelTools(ChannelBase channel)
        {
            InitializeComponent();
            DataContext = channel;
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider && slider.IsFocused)
            {
                //Console.WriteLine("SLIDER VALUE CHANGE: {0} - {1} ", e.OldValue, e.NewValue);

                ReportValueOfControl(slider);
            }

        }
        private void Slider_GotFocus(object sender, RoutedEventArgs e)
        {
            ReportValueOfControl((Slider)sender, true);
        }
        public void ReportValueOfControl(Slider control, bool includeNameFirst = false)
        {

            if (control is Slider slider)
            {
                BindingExpression be = BindingOperations.GetBindingExpression(slider, (Slider.ValueProperty));
                string Name = slider.Name;
                if (Name == string.Empty || Name == null) Name = (string?)slider.GetValue(AutomationProperties.NameProperty);
                if (includeNameFirst)
                    ReadTextToScreenReader(Name + " " + Math.Round(slider.Value, 2) + slider.Tag?.ToString());
                else
                    ReadTextToScreenReader(Math.Round(slider.Value, 2) + slider.Tag?.ToString());
            }
        }

        public void ReadTextToScreenReader(string text)
        {
            //if (oldText == text) return;
            this.screenReaderText.Text = text;
            var peer = UIElementAutomationPeer.FromElement(screenReaderText);
            if (peer == null) peer = UIElementAutomationPeer.CreatePeerForElement(screenReaderText);
            peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            //Console.WriteLine("SCREENREADER: " + text);

        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            if (e.Key == Key.J) ReadMeter();
        }

        CircularBuffer<float> meterValues = new CircularBuffer<float>(50);
        private void ClipChecked(object sender, RoutedEventArgs e)
        {

        }

        private void meterChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            meterValues.Enqueue((float)e.NewValue);
        }

        private void ReadMeter()
        {
            var val = meterValues.Max();
            ReadTextToScreenReader(Math.Round(val, 2) + " db");
        }
    }
}

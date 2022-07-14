using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Models;
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
    /// Interaction logic for SendsOnFadersPanel.xaml
    /// </summary>
    public partial class SendsOnFadersPanel : Window
    {
        public SendsOnFadersPanel(ChannelBase channel, Device device)
        {
            InitializeComponent();
            DataContext = new SendsOnFadersViewModel(channel, device);
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider && slider.IsFocused)
            {
                Console.WriteLine("SLIDER VALUE CHANGE: {0} - {1} ", e.OldValue, e.NewValue);

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
        }
    }
}

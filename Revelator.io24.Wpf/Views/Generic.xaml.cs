using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presonus.StudioLive32.Wpf.Views
{
    /// <summary>
    /// Interaction logic for Generic.xaml
    /// </summary>
    public partial class Generic : ResourceDictionary
    {
        public Generic()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider && slider.IsFocused)
            {
                ReportValueOfControl(slider);
            }
        }

        private void Slider_GotFocus(object sender, RoutedEventArgs e)
        {
            ReportValueOfControl((Slider)sender, true);

        }

        private void Slider_PreviewKeyDown(object sender, KeyEventArgs e)
        {

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
        public void ReadTextToScreenReader(string text)
        {
            //if (oldText == text) return;
            //this.screenReaderText.Text = text;
            // var peer = UIElementAutomationPeer.FromElement(screenReaderText);
            //if (peer != null)
            // {
            //      peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            //  }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

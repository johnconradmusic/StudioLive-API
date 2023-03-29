using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presonus.StudioLive32.Wpf.UserControls
{
    /// <summary>
    /// Interaction logic for ChannelStrip.xaml
    /// </summary>
    public partial class ChannelStrip : UserControl
    {
        public ChannelStrip()
        {
            InitializeComponent();
        }
		private void ShowGateParameters(object sender, RoutedEventArgs e)
		{
			GateParametersPopup.IsOpen = true;
		}
		private void ToggleEqPanelVisibility(object sender, RoutedEventArgs e)
		{
			if (EqPanel.Visibility == Visibility.Visible)
			{
				EqPanel.Visibility = Visibility.Collapsed;
			}
			else
			{
				EqPanel.Visibility = Visibility.Visible;
			}
		}

		private void ShowCompressorParameters(object sender, RoutedEventArgs e)
		{
			CompressorParametersPopup.IsOpen = true;
		}

		private void ShowLimiterParameters(object sender, RoutedEventArgs e)
		{
			LimiterParametersPopup.IsOpen = true;
		}

	}
}

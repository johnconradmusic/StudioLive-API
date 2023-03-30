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
			GateParametersPopup.InvalidateVisual();
		}
		private void ToggleEqPanelVisibility(object sender, RoutedEventArgs e)
		{
			EqPanel.IsOpen = true;
			EqPanel.InvalidateVisual();
		}

		private void ShowCompressorParameters(object sender, RoutedEventArgs e)
		{
			CompressorParametersPopup.IsOpen = true;
			CompressorParametersPopup.InvalidateVisual();
		}

		private void ShowLimiterParameters(object sender, RoutedEventArgs e)
		{
			LimiterParametersPopup.IsOpen = true;
			LimiterParametersPopup.InvalidateVisual();
		}

	}
}

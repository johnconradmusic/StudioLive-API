using Presonus.UCNet.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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

		private void SetAccessibleNames(DependencyObject element)
		{
			// Check if the element is a FrameworkElement and implements the IAccessibleControl interface
			if (element is FrameworkElement frameworkElement && frameworkElement is IAccessibleControl control)
			{
				// Set the accessible name using the Caption property value
				AutomationProperties.SetName(frameworkElement, control.Caption);
			}

			// Check if the element has child elements
			int childCount = VisualTreeHelper.GetChildrenCount(element);
			if (childCount > 0)
			{
				// Loop through all child elements and call this method recursively
				for (int i = 0; i < childCount; i++)
				{
					DependencyObject childElement = VisualTreeHelper.GetChild(element, i);
					SetAccessibleNames(childElement);
				}
			}
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			SetAccessibleNames(MainContainer);
		}

	}
}

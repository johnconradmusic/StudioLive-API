using Presonus.UCNet.Wpf.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;

namespace Presonus.UCNet.Wpf.UserControls
{
	/// <summary>
	/// Interaction logic for ChannelStrip.xaml
	/// </summary>
	public partial class ChannelStrip : UserControl, INotifyPropertyChanged
	{
		private string selectedControl;
		private string selectedValue;

		public ChannelStrip()
		{
			InitializeComponent();
			SetAccessibleNames(MainContainer);
			Loaded += (s, e) =>
			{
				TraverseVisualTree(this, AttachGotFocusEventHandler);
				SetAccessibleNames(MainContainer);
			};
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler SelectedControlChanged;

		public string SelectedControl
		{ get { return selectedControl; } set { Console.WriteLine(value); selectedControl = value; OnPropertyChanged(); } }

		public string SelectedValue
		{ get { return selectedValue; } set { selectedValue = value; OnPropertyChanged(); } }

		private void AttachGotFocusEventHandler(FrameworkElement element)
		{
			if (element is IAccessibleControl)
			{
				element.GotFocus += Control_GotFocus;
			}
		}

		private void Control_GotFocus(object sender, RoutedEventArgs e)
		{
			if (sender is IAccessibleControl control)
			{
				SelectedControl = control.Caption;
				SelectedValue = control.ValueString;
				//Console.WriteLine($"control got focus {control.Caption}");
			}
		}

		private void TraverseVisualTree(DependencyObject target, Action<FrameworkElement> action)
		{
			if (target == null) return;

			int childrenCount = VisualTreeHelper.GetChildrenCount(target);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(target, i);
				if (child is FrameworkElement frameworkElement)
				{
					action(frameworkElement);
				}
				TraverseVisualTree(child, action);
			}
		}

		private void ShowGateParameters(object sender, RoutedEventArgs e)
		{
			GateParametersPopup.IsOpen = true;
			GateParametersPopup.InvalidateVisual();
		}

		private void ToggleEqPanelVisibility(object sender, RoutedEventArgs e)
		{
			EqPanel.IsOpen = !EqPanel.IsOpen;
		}

		private void ShowCompressorParameters(object sender, RoutedEventArgs e)
		{
			CompressorParametersPopup.IsOpen = !CompressorParametersPopup.IsOpen;
		}

		private void ShowLimiterParameters(object sender, RoutedEventArgs e)
		{
			LimiterParametersPopup.IsOpen = !LimiterParametersPopup.IsOpen;
		}

		private void SetAccessibleNames(DependencyObject element)
		{
			// Check if the element is a FrameworkElement and implements the IAccessibleControl interface
			if (element is FrameworkElement frameworkElement && frameworkElement is IAccessibleControl control)
			{
				// Set the accessible name using the Caption property value
				//AutomationProperties.SetName(frameworkElement, control.Caption);
				control.ValueChanged += Control_ValueChanged;
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

		private void Control_ValueChanged(object? sender, EventArgs e)
		{
			//Console.WriteLine("changed");
			if (sender is IAccessibleControl control)
			{
				//Console.WriteLine("assigned");

				SelectedControl = control.Caption;
				SelectedValue = control.ValueString;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			AuxSendsPopup.IsOpen = !AuxSendsPopup.IsOpen;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnSelectedControlChanged()
		{
			SelectedControlChanged?.Invoke(this, EventArgs.Empty);
		}

		private void RoutingButton_Click(object sender, RoutedEventArgs e)
		{
			var state = RoutingPanel.Visibility;
			switch (state)
			{
				case Visibility.Visible:
					RoutingPanel.Visibility = Visibility.Collapsed;
					break;
				case Visibility.Hidden:
					break;
				case Visibility.Collapsed:
					RoutingPanel.Visibility = Visibility.Visible;
					break;
				default:
					break;
			}
		}
	}
}
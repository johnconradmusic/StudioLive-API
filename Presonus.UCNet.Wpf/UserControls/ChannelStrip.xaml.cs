using Microsoft.Extensions.DependencyInjection;
using Presonus.UCNet.Api;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Services;
using Presonus.UCNet.Wpf.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Media;
using static ICSharpCode.SharpZipLib.Zip.ZipEntryFactory;

namespace Presonus.UCNet.Wpf.UserControls
{
	/// <summary>
	/// Interaction logic for ChannelStrip.xaml
	/// </summary>
	public partial class ChannelStrip : UserControl, INotifyPropertyChanged
	{
		private string selectedControl;
		private string selectedValue;
		public float Meter => channel?.ChannelType != null && channel.ChannelIndex != null
			? _meterService.MeterData.GetData(new(channel.ChannelType, channel.ChannelIndex - 1))
			: 0.0f;
		public float Peak;
		private Channel channel;
		private MeterService _meterService;
		public ChannelStrip()
		{
			InitializeComponent();
			Loaded += (s, e) =>
			{
				SetAccessibleNames(MainContainer);
				TraverseVisualTree(this, AttachGotFocusEventHandler);
				SetAccessibleNames(MainContainer);
				_meterService = App.ServiceProvider.GetRequiredService<MeterService>();
				_meterService.MeterDataReceived += _meterService_MeterDataReceived;
				channel = channelStrip.DataContext as Channel;
			};

		}


		private void _meterService_MeterDataReceived(object? sender, MeterDataEventArgs e)
		{
			var value = ValueTransformer.LinearToMeter(Meter);
			if (value > Peak) Peak = value;
			else Peak *= 0.9f;
			meter.Value = Peak;
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

		private void SetAccessibleNames(DependencyObject element)
		{
			// Check if the element is a FrameworkElement and implements the IAccessibleControl interface
			if (element is FrameworkElement frameworkElement && frameworkElement is IAccessibleControl control)
			{
				// Set the accessible name using the Caption property value
				AutomationProperties.SetName((FrameworkElement)control, control.Caption + " " + control.ValueString);
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
			if (sender is IAccessibleControl control)
			{
				AutomationProperties.SetName((FrameworkElement)control, control.Caption + " " + control.ValueString);

				SelectedControl = control.Caption;
				SelectedValue = control.ValueString;
			}
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnSelectedControlChanged()
		{
			SelectedControlChanged?.Invoke(this, EventArgs.Empty);
		}

		private void channelStrip_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.M)
			{
				e.Handled = true;
				UpdateLiveRegionText(ValueTransformer.LinearToVolume((float)Peak).ToString());
			}
		}
		public void UpdateLiveRegionText(string newText)
		{
			liveRegionLabel.Text = newText;
			AutomationPeer peer = FrameworkElementAutomationPeer.FromElement(liveRegionLabel);
			if (peer == null)
			{
				peer = new FrameworkElementAutomationPeer(liveRegionLabel);
			}
			if (peer != null)
			{
				Console.WriteLine(newText);
				peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
			}
		}

	}
}
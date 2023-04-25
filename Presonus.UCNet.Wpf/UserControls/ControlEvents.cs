using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presonus.UCNet.Wpf
{
	public static class ControlEvents
	{
		public static readonly DependencyProperty GotFocusProperty =
			DependencyProperty.RegisterAttached("GotFocus", typeof(RoutedEventHandler), typeof(ControlEvents), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty ValueChangedProperty =
			DependencyProperty.RegisterAttached("ValueChanged", typeof(RoutedEventHandler), typeof(ControlEvents), new FrameworkPropertyMetadata(null));

		public static RoutedEventHandler GetGotFocus(DependencyObject obj)
		{
			return (RoutedEventHandler)obj.GetValue(GotFocusProperty);
		}

		public static void SetGotFocus(DependencyObject obj, RoutedEventHandler value)
		{
			obj.SetValue(GotFocusProperty, value);
		}

		public static RoutedEventHandler GetValueChanged(DependencyObject obj)
		{
			return (RoutedEventHandler)obj.GetValue(ValueChangedProperty);
		}

		public static void SetValueChanged(DependencyObject obj, RoutedEventHandler value)
		{
			obj.SetValue(ValueChangedProperty, value);
		}
	}

}

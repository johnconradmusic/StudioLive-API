using Presonus.UCNet.Wpf.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presonus.UCNet.Wpf.UserControls
{
	public partial class MiniToggle : UserControl, IAccessibleControl
	{
		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register("IsChecked", typeof(bool), typeof(MiniToggle), new PropertyMetadata(false, OnIsCheckedChanged));

		// Using a DependencyProperty as the backing store for Caption. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty CaptionProperty =
			DependencyProperty.Register("Caption", typeof(string), typeof(MiniToggle), new PropertyMetadata("unknown control"));

		// Using a DependencyProperty as the backing store for ValueString. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty ValueStringProperty =
			DependencyProperty.Register("ValueString", typeof(string), typeof(MiniToggle), new PropertyMetadata("unknown value"));

		public event EventHandler ValueChanged;
		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as MiniToggle;
			control.ValueString = control.IsChecked ? "On" : "Off";
			control?.ValueChanged?.Invoke(control, EventArgs.Empty);
		}
		public MiniToggle()
		{
			InitializeComponent();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ValueString = IsChecked ? "On" : "Off";
		}

		public string Caption
		{
			get { return (string)GetValue(CaptionProperty); }
			set { SetValue(CaptionProperty, value); }
		}

		public string ValueString
		{
			get { return (string)GetValue(ValueStringProperty); }
			set { SetValue(ValueStringProperty, value); }
		}

		public bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
		}
	}
}
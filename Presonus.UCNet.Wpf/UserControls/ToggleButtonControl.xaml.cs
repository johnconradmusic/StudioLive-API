using Presonus.UCNet.Wpf.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Presonus.UCNet.Wpf.UserControls
{
	public partial class ToggleButtonControl : UserControl, IAccessibleControl
	{
		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register("IsChecked", typeof(bool), typeof(ToggleButtonControl), new PropertyMetadata(false, OnIsCheckedChanged));

		public static readonly DependencyProperty UncheckedBackgroundProperty =
			DependencyProperty.Register("UncheckedBackground", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.LightGray));

		public static readonly DependencyProperty CheckedBackgroundProperty =
			DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.LightGreen));

		public static readonly DependencyProperty BorderBrushProperty =
			DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.Black));

		public static readonly DependencyProperty BorderThicknessProperty =
			DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(ToggleButtonControl), new PropertyMetadata(new Thickness(1)));

		public static readonly DependencyProperty UncheckedForegroundProperty =
			DependencyProperty.Register("UncheckedForeground", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.Black));

		public static readonly DependencyProperty CheckedForegroundProperty =
			DependencyProperty.Register("CheckedForeground", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.White));

		public static readonly DependencyProperty OutlineBrushProperty =
			DependencyProperty.Register("OutlineBrush", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.Black));

		public static readonly DependencyProperty OutlineThicknessProperty =
			DependencyProperty.Register("OutlineThickness", typeof(double), typeof(ToggleButtonControl), new PropertyMetadata(1.0));

		// Using a DependencyProperty as the backing store for Caption. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty CaptionProperty =
			DependencyProperty.Register("Caption", typeof(string), typeof(ToggleButtonControl), new PropertyMetadata(""));

		// Using a DependencyProperty as the backing store for ValueString. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty ValueStringProperty =
			DependencyProperty.Register("ValueString", typeof(string), typeof(ToggleButtonControl), new PropertyMetadata(""));

		public ToggleButtonControl()
		{
			InitializeComponent();
		}

		public event EventHandler ValueChanged;

		public Brush OutlineBrush
		{
			get { return (Brush)GetValue(OutlineBrushProperty); }
			set { SetValue(OutlineBrushProperty, value); }
		}

		public double OutlineThickness
		{
			get { return (double)GetValue(OutlineThicknessProperty); }
			set { SetValue(OutlineThicknessProperty, value); }
		}

		public bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
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

		public Brush UncheckedBackground
		{
			get { return (Brush)GetValue(UncheckedBackgroundProperty); }
			set { SetValue(UncheckedBackgroundProperty, value); }
		}

		public Brush CheckedBackground
		{
			get { return (Brush)GetValue(CheckedBackgroundProperty); }
			set { SetValue(CheckedBackgroundProperty, value); }
		}

		public Brush BorderBrush
		{
			get { return (Brush)GetValue(BorderBrushProperty); }
			set { SetValue(BorderBrushProperty, value); }
		}

		public Thickness BorderThickness
		{
			get { return (Thickness)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}

		public Brush UncheckedForeground
		{
			get { return (Brush)GetValue(UncheckedForegroundProperty); }
			set { SetValue(UncheckedForegroundProperty, value); }
		}

		public Brush CheckedForeground
		{
			get { return (Brush)GetValue(CheckedForegroundProperty); }
			set { SetValue(CheckedForegroundProperty, value); }
		}

		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ToggleButtonControl;
			control.ValueString = control.IsChecked ? "On" : "Off";
			control?.ValueChanged?.Invoke(control, EventArgs.Empty);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ValueString = IsChecked ? "On" : "Off";
		}
	}
}
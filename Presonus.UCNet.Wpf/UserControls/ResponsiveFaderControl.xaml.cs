using Presonus.UCNet.Api.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.UCNet.Wpf.UserControls
{
	public partial class ResponsiveFaderControl : UserControl
	{
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(float), typeof(ResponsiveFaderControl),
				new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(double), typeof(ResponsiveFaderControl),
				new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(double), typeof(ResponsiveFaderControl),
				new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty CaptionProperty =
			DependencyProperty.Register("Caption", typeof(string), typeof(ResponsiveFaderControl), new PropertyMetadata(""));

		public static readonly DependencyProperty ValueStringProperty =
			DependencyProperty.Register("ValueString", typeof(string), typeof(ResponsiveFaderControl), new PropertyMetadata(""));

		public ResponsiveFaderControl()
		{
			InitializeComponent();
		}

		public event EventHandler ValueChanged;

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

		public float Value
		{
			get => (float)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		public double Minimum
		{
			get => (double)GetValue(MinimumProperty);
			set => SetValue(MinimumProperty, value);
		}

		public double Maximum
		{
			get => (double)GetValue(MaximumProperty);
			set => SetValue(MaximumProperty, value);
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ResponsiveFaderControl;
			control?.UpdateDisplay();
			control?.ValueChanged?.Invoke(control, EventArgs.Empty);
		}

		private void UpdateDisplay()
		{
			ValueString = ValueTransformer.Transform(Value, 0.0001f, 1, CurveFormula.LinearToVolume, Units.DB);
		}

		private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			Value += (float)e.Delta / 120f / 50f;
			Value = Math.Clamp(Value, 0f, 1f);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			UpdateDisplay();
		}
	}
}
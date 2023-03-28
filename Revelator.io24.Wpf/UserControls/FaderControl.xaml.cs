using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presonus.StudioLive32.Wpf.UserControls
{
	public partial class FaderControl : UserControl
	{
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(float), typeof(FaderControl),
				new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(double), typeof(FaderControl),
				new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(double), typeof(FaderControl),
				new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


		public FaderControl()
		{
			InitializeComponent();
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as FaderControl;
			control?.UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			Console.WriteLine("Output: " + Math.Round(ValueTransformer.Transform(Value, 0.001, 1, CurveFormula.InverseLog),2));
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

		private void UserControl_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
		{
			Value += (float)e.Delta / 120f / 50f;
			Value = Math.Clamp(Value, 0f, 1f);
		}
	}

}
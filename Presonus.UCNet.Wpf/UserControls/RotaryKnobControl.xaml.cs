using Presonus.UCNet.Api.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.StudioLive32.Wpf.UserControls
{
	public partial class RotaryKnobControl : UserControl
	{
		private bool isDragging;

		private Point dragStartPoint;


		public CurveFormula Curve
		{
			get { return (CurveFormula)GetValue(CurveProperty); }
			set { SetValue(CurveProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Curve.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurveProperty =
			DependencyProperty.Register("Curve", typeof(CurveFormula), typeof(RotaryKnobControl), new PropertyMetadata(CurveFormula.Linear));


		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(float), typeof(RotaryKnobControl),
				new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

		// Using a DependencyProperty as the backing store for Min. This enables animation, styling,
		// binding, etc...
		public static readonly DependencyProperty MinProperty =
			DependencyProperty.Register("Min", typeof(float), typeof(RotaryKnobControl), new PropertyMetadata(0f));

		// Using a DependencyProperty as the backing store for Max. This enables animation, styling,
		// binding, etc...
		public static readonly DependencyProperty MaxProperty =
			DependencyProperty.Register("Max", typeof(float), typeof(RotaryKnobControl), new PropertyMetadata(0f));



		public string Caption
		{
			get { return (string)GetValue(CaptionProperty); }
			set { SetValue(CaptionProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CaptionProperty =
			DependencyProperty.Register("Caption", typeof(string), typeof(RotaryKnobControl), new PropertyMetadata(""));



		public RotaryKnobControl()
		{
			InitializeComponent();
		}

		public float Min
		{
			get { return (float)GetValue(MinProperty); }
			set { SetValue(MinProperty, value); }
		}

		public float Max
		{
			get { return (float)GetValue(MaxProperty); }
			set { SetValue(MaxProperty, value); }
		}
				public float Value
		{
			get { return (float)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as RotaryKnobControl;
			control?.UpdateRotateTransform();
		}

		private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
		{
			isDragging = true;
			dragStartPoint = e.GetPosition(this);
			CaptureMouse();
		}

		private void Knob_MouseMove(object sender, MouseEventArgs e)
		{
			if (isDragging)
			{
				Point currentPosition = e.GetPosition(this);
				double distance = currentPosition.Y - dragStartPoint.Y;
				Value = (float)Math.Max(0, Math.Min(1, Value - distance / 500));
				dragStartPoint = currentPosition;
			}
		}

		private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
		{
			isDragging = false;
			ReleaseMouseCapture();
		}

		private void Knob_MouseLeave(object sender, MouseEventArgs e)
		{
			isDragging = false;
			ReleaseMouseCapture();
		}

		private void UpdateRotateTransform()
		{
			double angleRange = 310; // The range of angles (in degrees) between 30 and 330
			double angleOffset = 20; // The offset angle (in degrees) for the minimum value
			double angle = angleOffset + Value * angleRange;
			KnobRotateTransform.Angle = angle;
			ValueText.Text = Math.Round(ValueTransformer.Transform(Value, Min, Max, Curve)).ToString();
		}

		private void RotaryKnob_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			Value += (float)e.Delta / 120f / 50f;
			Value = Math.Clamp(Value, 0f, 1f);
		}
	}
}
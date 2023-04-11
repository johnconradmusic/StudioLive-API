using Presonus.UCNet.Wpf.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.UCNet.Wpf.UserControls
{
	public partial class PanPotControl : UserControl, IAccessibleControl
	{
		private bool isDragging;

		private Point dragStartPoint;

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(float), typeof(PanPotControl),
				new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

		public static readonly DependencyProperty CaptionProperty =
			DependencyProperty.Register("Caption", typeof(string), typeof(PanPotControl), new PropertyMetadata(""));

		public static readonly DependencyProperty ValueStringProperty =
			DependencyProperty.Register("ValueString", typeof(string), typeof(PanPotControl), new PropertyMetadata(""));

		public event EventHandler ValueChanged;

		public PanPotControl()
		{
			InitializeComponent();
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

		void UpdateValueString(float Value)
		{
			int panPercentage;
			float centerTolerance = 0.01f; // Tolerance range for the center

			// Check if the value is within the center tolerance range
			if (Math.Abs(Value - 0.5f) <= centerTolerance)
			{
				ValueString = "Center";
			}
			// If the value is less than 0.5, it means pan to the left
			else if (Value < 0.5f)
			{
				panPercentage = (int)((0.5f - Value) * 200); // Calculate the percentage for left panning
				ValueString = $"Pan Left {panPercentage}%";
			}
			// If the value is greater than 0.5, it means pan to the right
			else
			{
				panPercentage = (int)((Value - 0.5f) * 200); // Calculate the percentage for right panning
				ValueString = $"Pan Right {panPercentage}%";
			}
		}


		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			UpdateValueString(Value);
		}

		public float Value
		{
			get { return (float)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as PanPotControl;
			float newValue = (float)e.NewValue;
			float centerTolerance = 0.01f; // Tolerance range for the center

			// Snap the value to 0.5 if it's within the center tolerance range
			if (Math.Abs(newValue - 0.5f) <= centerTolerance)
			{
				newValue = 0.5f;
				// Update the dependency property with the new value
				control.SetValue(ValueProperty, newValue);
			}

			control?.UpdateRotateTransform();
			control?.UpdateValueString(newValue);
			control?.ValueChanged?.Invoke(control, EventArgs.Empty);
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
		}

		private void RotaryKnob_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			Value += (float)e.Delta / 120f / 50f;
			Value = Math.Clamp(Value, 0f, 1f);
		}
	}
}
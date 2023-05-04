using Presonus.UCNet.Api.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.UCNet.Wpf.Blind.UserControls
{
	/// <summary>
	/// Interaction logic for NumericUpDown.xaml
	/// </summary>
	public partial class NumericUpDown : UserControl
	{
		public static readonly DependencyProperty CurveProperty =
			DependencyProperty.Register("Curve", typeof(CurveFormula), typeof(NumericUpDown), new PropertyMetadata(CurveFormula.Linear));

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(float), typeof(NumericUpDown),
				new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

		// Using a DependencyProperty as the backing store for Min. This enables animation, styling,
		// binding, etc...
		public static readonly DependencyProperty MinProperty =
			DependencyProperty.Register("Min", typeof(float), typeof(NumericUpDown), new PropertyMetadata(0f));

		// Using a DependencyProperty as the backing store for Max. This enables animation, styling,
		// binding, etc...
		public static readonly DependencyProperty MaxProperty =
			DependencyProperty.Register("Max", typeof(float), typeof(NumericUpDown), new PropertyMetadata(0f));

		// Using a DependencyProperty as the backing store for Mid. This enables animation, styling,
		// binding, etc...
		public static readonly DependencyProperty MidProperty =
			DependencyProperty.Register("Mid", typeof(float), typeof(NumericUpDown), new PropertyMetadata(0f));

		// Using a DependencyProperty as the backing store for ValueString. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty ValueStringProperty =
			DependencyProperty.Register("ValueString", typeof(string), typeof(NumericUpDown), new PropertyMetadata("unknown value"));

		// Using a DependencyProperty as the backing store for Caption. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty CaptionProperty =
			DependencyProperty.Register("Caption", typeof(string), typeof(NumericUpDown), new PropertyMetadata(""));

		// Using a DependencyProperty as the backing store for Unit. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty UnitProperty =
			DependencyProperty.Register("Unit", typeof(Units), typeof(NumericUpDown), new PropertyMetadata(Units.NONE));



		public float Default
		{
			get { return (float)GetValue(DefaultProperty); }
			set { SetValue(DefaultProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Default.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DefaultProperty =
			DependencyProperty.Register("Default", typeof(float), typeof(NumericUpDown), new PropertyMetadata(0f));



		public NumericUpDown()
		{
			InitializeComponent();
		}

		public event EventHandler ValueChanged;

		public CurveFormula Curve
		{
			get { return (CurveFormula)GetValue(CurveProperty); }
			set { SetValue(CurveProperty, value); }
		}

		public float Mid
		{
			get { return (float)GetValue(MidProperty); }
			set { SetValue(MidProperty, value); }
		}

		public string ValueString
		{
			get { return (string)GetValue(ValueStringProperty); }
			set { SetValue(ValueStringProperty, value); }
		}

		public string Caption
		{
			get { return (string)GetValue(CaptionProperty); }
			set { SetValue(CaptionProperty, value); }
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

		public Units Unit
		{
			get { return (Units)GetValue(UnitProperty); }
			set { SetValue(UnitProperty, value); }
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as NumericUpDown;
			control.UpdateValueString();
			control.ValueChanged?.Invoke(control, EventArgs.Empty);
		}

		private void UpdateValueString()
		{
			ValueString = ValueTransformer.Transform(Value, Min, Max, Curve, Unit);
			if (IsFocused)
				Speech.SpeechManager.Say($"{ValueString}");
		}

		private void RotaryKnob_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			float delta = 0f;
			if (e.Key == Key.Enter)
			{
				Speech.SpeechManager.Say($"{ValueString}");
			}
			if (e.Key == Key.Delete)
			{
				Value = Default;
			}
			if (e.Key == Key.Up)
			{
				e.Handled = true;
				delta = 0.01f;
			}
			else if (e.Key == Key.Down)
			{
				e.Handled = true;
				delta = -0.01f;
			}
			else if (e.Key == Key.PageUp)
			{
				e.Handled = true;
				delta = 0.1f;
			}
			else if (e.Key == Key.PageDown)
			{
				e.Handled = true;
				delta = -0.1f;
			}
			if (ModifierKeys.IsCtrlDown()) delta /= 10;
			if (delta != 0f)
			{
				float newValue = Math.Clamp(Value + delta, 0f, 1f);
				if (newValue != Value)
				{
					Value = newValue;
				}
			}
		}

		private void UserControl_GotFocus(object sender, RoutedEventArgs e)
		{
			Speech.SpeechManager.Say($"{Caption} ({ValueString})");
		}
	}
}
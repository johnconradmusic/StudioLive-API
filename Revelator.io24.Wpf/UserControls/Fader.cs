using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presonus.StudioLive32.Wpf.UserControls
{
	/// <summary>
	/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	///
	/// Step 1a) Using this custom control in a XAML file that exists in the current project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:Presonus.StudioLive32.Wpf.UserControls"
	///
	///
	/// Step 1b) Using this custom control in a XAML file that exists in a different project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:Presonus.StudioLive32.Wpf.UserControls;assembly=Presonus.StudioLive32.Wpf.UserControls"
	///
	/// You will also need to add a project reference from the project where the XAML file lives
	/// to this project and Rebuild to avoid compilation errors:
	///
	///     Right click on the target project in the Solution Explorer and
	///     "Add Reference"->"Projects"->[Browse to and select this project]
	///
	///
	/// Step 2)
	/// Go ahead and use your control in the XAML file.
	///
	///     <MyNamespace:Fader/>
	///
	/// </summary>
	/// 
	public class StringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? "" : value.ToString();

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
	public class Fader : Control
	{

		public override void OnApplyTemplate()
		{
			Value = Default;
			base.OnApplyTemplate();
		}

		public static readonly RoutedEvent ValueChangedEvent
			= EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Fader));
		public event RoutedEventHandler ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		public static readonly RoutedEvent TextChangedEvent
	= EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Fader));
		public event RoutedEventHandler TextChanged
		{
			add { AddHandler(TextChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}



		public double Default
		{
			get { return (double)GetValue(DefaultProperty); }
			set { SetValue(DefaultProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Default.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DefaultProperty =
			DependencyProperty.Register("Default", typeof(double), typeof(Fader), new PropertyMetadata(default(double)));



		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(double), typeof(Fader), new PropertyMetadata(default(double)));

		public double Minimum
		{
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		// Using a DependencyProperty as the backing store for .  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(double), typeof(Fader), new PropertyMetadata(default(double)));


		public static readonly DependencyProperty ValueProperty
			= DependencyProperty.Register(nameof(Value), typeof(double), typeof(Fader), new PropertyMetadata(default(double)));
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set
			{
				if (value > Maximum) value = Maximum;

				if (value < Minimum) value = Minimum;

				SetValue(ValueProperty, value);
				RoutedEventArgs args = new RoutedEventArgs(ValueChangedEvent);
				RaiseEvent(args);

			}
		}
		public static readonly DependencyProperty UnitProperty
			= DependencyProperty.Register(nameof(Unit), typeof(string), typeof(Fader), new PropertyMetadata(default(string)));
		public string Unit
		{
			get { return (string)GetValue(UnitProperty); }
			set { SetValue(UnitProperty, value); }
		}
		enum ValueStepSize
		{
			Small = 1, Normal = 2, Large = 5
		}

		void Increment(ValueStepSize stepSize)
		{
			Value += GetIncrement * (int)stepSize;
		}

		void Decrement(ValueStepSize stepSize)
		{
			Value -= GetIncrement * (int)stepSize;
		}


		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Up)
			{
				e.Handled = true;
				Increment(ValueStepSize.Normal);
			}
			if (e.Key == Key.Down)
			{
				e.Handled = true;
				Decrement(ValueStepSize.Normal);
			}
			if (e.Key == Key.PageUp)
			{
				e.Handled = true;
				Increment(ValueStepSize.Large);
			}
			if (e.Key == Key.PageDown)
			{
				e.Handled = true;
				Decrement(ValueStepSize.Large);
			}
			if (e.Key == Key.Enter)
			{
				e.Handled = true;
				var textField = Template.FindName("TextField", this) as TextBox;
				if (textField != null)
				{
					if (double.TryParse(textField.Text, out double val))
					{
						Value = val;
					}
				}
			}
			if (e.Key == Key.Delete)
			{
				e.Handled = true;
				Value = Default;
			}
			//base.OnPreviewKeyDown(e);
		}

		private double GetIncrement
		{
			get
			{
				return Maximum / 50;
			}
		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			var sign = Math.Sign(e.Delta);
			if (sign > 0) Value += GetIncrement;
			else if (sign < 0) Value -= GetIncrement;
			base.OnMouseWheel(e);
		}
	}
}

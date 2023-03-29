using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Presonus.StudioLive32.Wpf.UserControls
{
	public class ObservableControl<T> : UserControl
	{
		private static ValueDisplay _valueDisplay;

		public static void SetGlobalValueDisplay(ValueDisplay valueDisplay)
		{
			_valueDisplay = valueDisplay;
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(T), typeof(ObservableControl<T>), new PropertyMetadata(default(T), OnValueChanged));

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ObservableControl<T>;
			if (control != null && _valueDisplay != null)
			{
				_valueDisplay.Value = e.NewValue?.ToString();
			}

			control.OnValueChanged((T)e.NewValue);
		}

		public T Value
		{
			get { return (T)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		protected virtual void OnValueChanged(T newValue)
		{
			ValueChanged?.Invoke(this, newValue);
		}

		public event EventHandler<T> ValueChanged;
	}

}

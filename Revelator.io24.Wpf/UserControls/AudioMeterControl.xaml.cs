using System.Windows;
using System.Windows.Controls;

namespace Presonus.StudioLive32.Wpf.UserControls
{
	public partial class AudioMeterControl : UserControl
	{
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(AudioMeterControl),
				new PropertyMetadata(0d, OnValueChanged));

		public static readonly DependencyProperty PeakProperty =
			DependencyProperty.Register("Peak", typeof(double), typeof(AudioMeterControl),
				new PropertyMetadata(0d, OnPeakChanged));

		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public double Peak
		{
			get { return (double)GetValue(PeakProperty); }
			set { SetValue(PeakProperty, value); }
		}

		public AudioMeterControl()
		{
			InitializeComponent();
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = (AudioMeterControl)d;
			control.UpdateMeterBar();
		}

		private static void OnPeakChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = (AudioMeterControl)d;
			control.UpdatePeakIndicator();
		}

		private void UpdateMeterBar()
		{
			MeterBar.Height = ActualHeight * (Value / 100);
		}

		private void UpdatePeakIndicator()
		{
			PeakIndicator.Margin = new Thickness(0, 0, 0, ActualHeight * (Peak / 100));
		}
	}

}

using Presonus.UCNet.Api.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Presonus.UCNet.Wpf.UserControls
{
	/// <summary>
	/// Interaction logic for MeterControl.xaml
	/// </summary>
	public partial class MeterControl : UserControl
	{
		private const double DecayIntervalMs = 20;

		private DispatcherTimer _decayTimer;

		// Using a DependencyProperty as the backing store for Clip. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty ClipProperty =
			DependencyProperty.Register("Clip", typeof(bool), typeof(MeterControl), new PropertyMetadata(false));

		// Using a DependencyProperty as the backing store for DecayRate. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty DecayRateProperty =
			DependencyProperty.Register("DecayRate", typeof(float), typeof(MeterControl), new PropertyMetadata(0.01f));

		// Using a DependencyProperty as the backing store for Value. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(float), typeof(MeterControl), new PropertyMetadata(0f, OnValueChanged));

		public MeterControl()
		{
			Console.WriteLine("meter constructed");
			InitializeComponent();
			_decayTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(DecayIntervalMs), DispatcherPriority.Background, DecayTimer_Tick, Dispatcher);
		}

		public new bool Clip
		{
			get { return (bool)GetValue(ClipProperty); }
			set { SetValue(ClipProperty, value); }
		}

		public float DecayRate
		{
			get { return (float)GetValue(DecayRateProperty); }
			set { SetValue(DecayRateProperty, value); }
		}

		public float Value
		{
			get { return (float)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var meterControl = (MeterControl)d;
			var newValue = (float)e.NewValue;
			if (newValue > meterControl.ProgressBar.Value)
			{
				meterControl.ProgressBar.Value = newValue > 0 ? ValueTransformer.LinearToMeter(newValue) : 0f;
			}
		}

		private void DecayTimer_Tick(object sender, EventArgs e)
		{
			ProgressBar.Value -= DecayRate;
		}
	}
}
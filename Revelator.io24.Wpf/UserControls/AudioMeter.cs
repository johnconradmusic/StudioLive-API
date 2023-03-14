//------------------------------------------------------------------------------
// The Assistant - Copyright (c) 2016-2023, John Conrad
//------------------------------------------------------------------------------
using System.Windows;
using System.Windows.Controls;

namespace Presonus.StudioLive32.Wpf.UserControls
{
	internal class AudioMeter : Control
	{
		// Using a DependencyProperty as the backing store for Sections. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty SectionsProperty =
			DependencyProperty.Register("Sections", typeof(int), typeof(AudioMeter), new PropertyMetadata(default(AudioMeter)));

		// Using a DependencyProperty as the backing store for Value. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(float), typeof(AudioMeter), new PropertyMetadata(default(AudioMeter)));

		// Using a DependencyProperty as the backing store for PeakValue. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty PeakValueProperty =
			DependencyProperty.Register("PeakValue", typeof(float), typeof(AudioMeter), new PropertyMetadata(default(AudioMeter)));

		public int Sections
		{
			get { return (int)GetValue(SectionsProperty); }
			set { SetValue(SectionsProperty, value); }
		}

		public float Value
		{
			get { return (float)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public float PeakValue
		{
			get { return (float)GetValue(PeakValueProperty); }
			set { SetValue(PeakValueProperty, value); }
		}
	}
}
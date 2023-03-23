using Presonus.UCNet.Api.Services;
using System.Windows;
using System.Windows.Controls;

namespace Presonus.StudioLive32.Wpf.UserControls
{
	public partial class FaderControl : UserControl
	{
		private MixerStateService _mixerStateService;

		public static readonly DependencyProperty MinimumProperty =
					DependencyProperty.Register("Minimum", typeof(double), typeof(FaderControl),
				new PropertyMetadata(0d, (d, e) => ((FaderControl)d).FaderSlider.Minimum = (double)e.NewValue));

		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(double), typeof(FaderControl),
				new PropertyMetadata(1d, (d, e) => ((FaderControl)d).FaderSlider.Maximum = (double)e.NewValue));

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(FaderControl),
				new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty PathProperty =
	DependencyProperty.Register("Path", typeof(string), typeof(FaderControl),
		new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
			(d, e) => ((FaderControl)d).UpdateValueFromPath((string)e.NewValue)));

		public FaderControl()
		{
			InitializeComponent();
			FaderSlider.ValueChanged += (s, e) =>
			{
				SetValue(ValueProperty, e.NewValue);
				if (Path != null)
				{
					_mixerStateService.SetValue(Path, (float)e.NewValue);
				}
			};
		}

		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}

		public double Minimum
		{
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private void UpdateValueFromPath(string path)
		{
			if (path != null)
			{
				Value = _mixerStateService.GetValue(path);
			}
		}
	}
}
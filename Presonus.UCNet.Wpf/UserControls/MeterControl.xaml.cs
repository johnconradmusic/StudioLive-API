using Presonus.UCNet.Api.Helpers;
using System;
using System.Collections.Generic;
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

namespace Presonus.UCNet.Wpf.UserControls
{
	/// <summary>
	/// Interaction logic for MeterControl.xaml
	/// </summary>
	public partial class MeterControl : UserControl
	{
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			nameof(Value),
			typeof(float),
			typeof(MeterControl),
			new PropertyMetadata(0f, OnValueChanged));

		public float Value
		{
			get { return (float)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var meterControl = (MeterControl)d;
			meterControl.UpdateMeterImage();
		}

		private void UpdateMeterImage()
		{
			var onStateHeight = meterOnImage.ActualHeight;
			var onStateWidth = meterOnImage.ActualWidth;
			var portionToShow = ValueTransformer.Transform(Value, 0, 1, CurveFormula.LinearToVolume) +84;
			Console.WriteLine($"{Value} -> {portionToShow}");
			var clipGeometry = new RectangleGeometry(new Rect(0, onStateHeight - (onStateHeight * portionToShow), onStateWidth, onStateHeight * portionToShow));
			meterOnImage.Clip = clipGeometry;			
		}

		public MeterControl()
		{
			InitializeComponent();
		}
	}


}

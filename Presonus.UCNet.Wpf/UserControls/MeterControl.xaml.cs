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
			Console.WriteLine($"meter {Value}");
		}

		public MeterControl()
		{
			InitializeComponent();
		}
	}


}

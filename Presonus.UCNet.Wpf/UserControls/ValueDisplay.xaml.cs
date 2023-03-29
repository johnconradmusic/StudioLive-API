using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.StudioLive32.Wpf.UserControls
{
	public partial class ValueDisplay : UserControl
	{
		public ValueDisplay()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(string), typeof(ValueDisplay), new PropertyMetadata(""));

		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
	}

}

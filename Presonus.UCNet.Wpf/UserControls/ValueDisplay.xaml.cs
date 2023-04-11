using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.UCNet.Wpf.UserControls
{
	public partial class ValueDisplay : UserControl
	{
		public string ControlName
		{
			get { return (string)GetValue(ControlNameProperty); }
			set { SetValue(ControlNameProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ControlName.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ControlNameProperty =
			DependencyProperty.Register("ControlName", typeof(string), typeof(ValueDisplay), new PropertyMetadata(""));
		

		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(string), typeof(ValueDisplay), new PropertyMetadata(""));

		public ValueDisplay()
		{
			InitializeComponent();
		}
	}


}

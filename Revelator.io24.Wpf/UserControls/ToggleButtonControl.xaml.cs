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

namespace Presonus.StudioLive32.Wpf.UserControls
{
	public partial class ToggleButtonControl : UserControl
	{
		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register("IsChecked", typeof(bool), typeof(ToggleButtonControl), new PropertyMetadata(false));

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(ToggleButtonControl), new PropertyMetadata(string.Empty));

		public static readonly DependencyProperty CheckedColorProperty =
			DependencyProperty.Register("CheckedColor", typeof(SolidColorBrush), typeof(ToggleButtonControl), new PropertyMetadata(new SolidColorBrush(Colors.Green)));

		public bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
		}

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public SolidColorBrush CheckedColor
		{
			get { return (SolidColorBrush)GetValue(CheckedColorProperty); }
			set { SetValue(CheckedColorProperty, value); }
		}

		public ToggleButtonControl()
		{
			InitializeComponent();
			ToggleButtonBorder.MouseDown += ToggleButtonBorder_MouseDown;
		}

		private void ToggleButtonBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			IsChecked = !IsChecked;
		}
	}
}

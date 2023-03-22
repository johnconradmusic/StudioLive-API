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
	public partial class ScribbleStripControl : UserControl
	{
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(ScribbleStripControl), new PropertyMetadata("Text"));

		public static readonly DependencyProperty StripColorProperty =
			DependencyProperty.Register("StripColor", typeof(SolidColorBrush), typeof(ScribbleStripControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public SolidColorBrush StripColor
		{
			get { return (SolidColorBrush)GetValue(StripColorProperty); }
			set { SetValue(StripColorProperty, value); }
		}

		public ScribbleStripControl()
		{
			InitializeComponent();
		}
	}
}

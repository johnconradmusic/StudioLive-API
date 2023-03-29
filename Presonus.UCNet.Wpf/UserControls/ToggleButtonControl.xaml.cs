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

		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(object), typeof(ToggleButtonControl), new PropertyMetadata(null));

		public static readonly DependencyProperty UncheckedBackgroundProperty =
			DependencyProperty.Register("UncheckedBackground", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.LightGray));

		public static readonly DependencyProperty CheckedBackgroundProperty =
			DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.LightGreen));

		public static readonly DependencyProperty BorderBrushProperty =
			DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.Black));

		public static readonly DependencyProperty BorderThicknessProperty =
			DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(ToggleButtonControl), new PropertyMetadata(new Thickness(1)));

		public static readonly DependencyProperty UncheckedForegroundProperty =
			DependencyProperty.Register("UncheckedForeground", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.Black));

		public static readonly DependencyProperty CheckedForegroundProperty =
			DependencyProperty.Register("CheckedForeground", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.White));

		public static readonly DependencyProperty OutlineBrushProperty =
			DependencyProperty.Register("OutlineBrush", typeof(Brush), typeof(ToggleButtonControl), new PropertyMetadata(Brushes.Black));

		public static readonly DependencyProperty OutlineThicknessProperty =
			DependencyProperty.Register("OutlineThickness", typeof(double), typeof(ToggleButtonControl), new PropertyMetadata(1.0));

		public Brush OutlineBrush
		{
			get { return (Brush)GetValue(OutlineBrushProperty); }
			set { SetValue(OutlineBrushProperty, value); }
		}

		public double OutlineThickness
		{
			get { return (double)GetValue(OutlineThicknessProperty); }
			set { SetValue(OutlineThicknessProperty, value); }
		}

		public bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
		}

		public object Content
		{
			get { return (object)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		public Brush UncheckedBackground
		{
			get { return (Brush)GetValue(UncheckedBackgroundProperty); }
			set { SetValue(UncheckedBackgroundProperty, value); }
		}

		public Brush CheckedBackground
		{
			get { return (Brush)GetValue(CheckedBackgroundProperty); }
			set { SetValue(CheckedBackgroundProperty, value); }
		}

		public Brush BorderBrush
		{
			get { return (Brush)GetValue(BorderBrushProperty); }
			set { SetValue(BorderBrushProperty, value); }
		}

		public Thickness BorderThickness
		{
			get { return (Thickness)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}

		public Brush UncheckedForeground
		{
			get { return (Brush)GetValue(UncheckedForegroundProperty); }
			set { SetValue(UncheckedForegroundProperty, value); }
		}

		public Brush CheckedForeground
		{
			get { return (Brush)GetValue(CheckedForegroundProperty); }
			set
			{
				SetValue(CheckedForegroundProperty, value);
			}
		}
		public ToggleButtonControl()
		{
			InitializeComponent();
		}
	}
}

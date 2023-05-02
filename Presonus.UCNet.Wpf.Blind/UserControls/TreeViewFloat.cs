using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace Presonus.UCNet.Wpf.Blind.UserControls
{
	public class TreeViewFloat : TreeViewItem
	{
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(
				"Value", typeof(float), typeof(TreeViewFloat));

		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register(
				"Minimum", typeof(float), typeof(TreeViewFloat));

		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register(
				"Maximum", typeof(float), typeof(TreeViewFloat));

		public float Value
		{
			get { return (float)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public float Minimum
		{
			get { return (float)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		public float Maximum
		{
			get { return (float)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		public TreeViewFloat()
		{
			var textBlock = new TextBlock();
			var binding = new MultiBinding();
			binding.StringFormat = "{0}: {1:F2}";
			binding.Bindings.Add(new Binding("Header") { Source = this });
			binding.Bindings.Add(new Binding("Value") { Source = this });
			textBlock.SetBinding(TextBlock.TextProperty, binding);
			Header = textBlock.Text;

			var slider = new Slider();
			slider.Width = 80;
			slider.Minimum = Minimum;
			slider.Maximum = Maximum;
			slider.SetBinding(Slider.ValueProperty, new Binding("Value") { Source = this, Mode = BindingMode.TwoWay });
			Items.Add(slider);
		}
	}
}

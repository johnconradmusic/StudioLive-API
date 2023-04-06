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
	public partial class EnumComboBox : UserControl
	{
		public static readonly DependencyProperty EnumTypeProperty =
			DependencyProperty.Register("EnumType", typeof(Type), typeof(EnumComboBox), new PropertyMetadata(null, OnEnumTypeChanged));

		public static readonly DependencyProperty SelectedEnumValueProperty =
			DependencyProperty.Register("SelectedEnumValue", typeof(Enum), typeof(EnumComboBox), new PropertyMetadata(null, OnSelectedEnumValueChanged));

		public static readonly DependencyProperty FloatValueProperty =
			DependencyProperty.Register("FloatValue", typeof(float), typeof(EnumComboBox), new PropertyMetadata(0f, OnFloatValueChanged));

		public Type EnumType
		{
			get { return (Type)GetValue(EnumTypeProperty); }
			set { SetValue(EnumTypeProperty, value); }
		}

		public Enum SelectedEnumValue
		{
			get { return (Enum)GetValue(SelectedEnumValueProperty); }
			set { SetValue(SelectedEnumValueProperty, value); }
		}

		public float FloatValue
		{
			get { return (float)GetValue(FloatValueProperty); }
			set { SetValue(FloatValueProperty, value); }
		}

		public EnumComboBox()
		{
			InitializeComponent();
		}

		private static void OnEnumTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var enumComboBox = (EnumComboBox)d;
			enumComboBox.ComboBox.ItemsSource = Enum.GetValues((Type)e.NewValue);
		}

		private static void OnSelectedEnumValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var enumComboBox = (EnumComboBox)d;
			var values = Enum.GetValues(enumComboBox.EnumType);
			var index = Array.IndexOf(values, e.NewValue);

			// Handle negative index values
			if (index < 0)
			{
				index = 0;
			}

			var floatValue = (float)index / (values.Length - 1);
			enumComboBox.FloatValue = floatValue;
		}


		private static void OnFloatValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var enumComboBox = (EnumComboBox)d;
			var values = Enum.GetValues(enumComboBox.EnumType);
			var index = (int)Math.Round((float)e.NewValue * (values.Length - 1));
			var selectedEnumValue = values.GetValue(index);
			enumComboBox.SelectedEnumValue = (Enum)selectedEnumValue;
		}
	}
}

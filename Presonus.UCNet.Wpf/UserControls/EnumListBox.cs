using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using Presonus.UCNet.Wpf.Converters;

namespace Presonus.UCNet.Wpf.UserControls
{
	public partial class EnumListBox<T> : ListBox where T : Enum
	{
		public EnumListBox() : base()
		{

			this.SetBinding(ItemsSourceProperty, new Binding
			{
				Source = this,
				Path = new PropertyPath(nameof(EnumType)),
				Converter = new FloatToEnumValuesConverter<T>()
			});

			this.SetBinding(SelectedIndexProperty, new Binding
			{
				Path = new PropertyPath(nameof(MyFloatValue)),
				Mode = BindingMode.TwoWay,
				Converter = new FloatToEnumValuesConverter<T>()
			});
		}

		public static readonly DependencyProperty EnumTypeProperty =
			DependencyProperty.Register(nameof(EnumType), typeof(Type), typeof(EnumListBox<T>), new PropertyMetadata(default(T), EnumTypeChanged));

		public Type EnumType
		{
			get { return (Type)GetValue(EnumTypeProperty); }
			set { SetValue(EnumTypeProperty, value); }
		}

		public static readonly DependencyProperty MyFloatValueProperty =
			DependencyProperty.Register(nameof(MyFloatValue), typeof(float), typeof(EnumListBox<T>), new PropertyMetadata(0f, MyFloatValueChanged));

		public float MyFloatValue
		{
			get { return (float)GetValue(MyFloatValueProperty); }
			set { SetValue(MyFloatValueProperty, value); }
		}

		private static void EnumTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Do something when the EnumType property changes.
		}

		private static void MyFloatValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Do something when the MyFloatValue property changes.
		}
	}


}

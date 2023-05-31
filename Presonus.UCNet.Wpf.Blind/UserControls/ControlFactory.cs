using Presonus.UCNet.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Presonus.UCNet.Wpf.Blind.UserControls
{
	public class ControlFactory
	{
		public static NumericUpDown CreateNumericUpDownControl(string caption, float minValue, float maxValue, float def, Units unit, CurveFormula curve, string bindingPath)
		{
			var numericUpDown = new NumericUpDown();
			numericUpDown.Caption = caption;
			numericUpDown.Min = minValue;
			numericUpDown.Max = maxValue;
			numericUpDown.Unit = unit;
			numericUpDown.Curve = curve;
			numericUpDown.Default = def;
			numericUpDown.SetBinding(NumericUpDown.ValueProperty, new Binding(bindingPath));
			return numericUpDown;
		}

		public static NumericUpDown CreateNumericUpDownControl(Panel panel, string caption, float minValue, float maxValue, float def, Units unit, CurveFormula curve, string bindingPath)
		{
			var numericUpDown = new NumericUpDown();
			numericUpDown.Caption = caption;
			numericUpDown.Min = minValue;
			numericUpDown.Max = maxValue;
			numericUpDown.Unit = unit;
			numericUpDown.Curve = curve;
			numericUpDown.Default = def;
			numericUpDown.SetBinding(NumericUpDown.ValueProperty, new Binding(bindingPath));
			panel.Children.Add(numericUpDown);
			return numericUpDown;
		}

		public static BooleanUpDown CreateBooleanUpDown(string caption, string bindingPath)
		{
			var upDown = new BooleanUpDown();
			upDown.Caption = caption;
			upDown.SetBinding(BooleanUpDown.ValueProperty, new Binding(bindingPath));
			return upDown;
		}

		public static BooleanUpDown CreateBooleanUpDown(Panel panel, string caption, string bindingPath)
		{
			var upDown = new BooleanUpDown();
			upDown.Caption = caption;
			upDown.SetBinding(BooleanUpDown.ValueProperty, new Binding(bindingPath));
			panel.Children.Add(upDown);
			return upDown;
		}

		public static ListUpDown CreateListUpDown(string caption, List<string> items, string bindingPath)
		{
			var listUpDown = new ListUpDown();
			listUpDown.Caption = caption;
			listUpDown.Items = items;
			listUpDown.SetBinding(ListUpDown.ValueProperty, new Binding(bindingPath));
			return listUpDown;
		}
		public static ListUpDown CreateListUpDown(Panel panel, string caption, List<string> items, string bindingPath)
		{
			var listUpDown = new ListUpDown();
			listUpDown.Caption = caption;
			listUpDown.Items = items;
			listUpDown.SetBinding(ListUpDown.ValueProperty, new Binding(bindingPath));
			panel.Children.Add(listUpDown);
			return listUpDown;
		}
	}
}

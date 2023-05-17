using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Api;
using Presonus.UCNet.Wpf.Blind.UserControls;
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
using System.Windows.Shapes;
using Presonus.UCNet.Api.Helpers;

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
	/// <summary>
	/// Interaction logic for GEQWindow.xaml
	/// </summary>
	public partial class GEQWindow : ToolWindow
	{

		public GEQWindow(GEQ geq)
		{
			DataContext = geq;
			InitializeComponent();
			Title = $"Graphic EQ Window - Source";
			Loaded += FXComponentWindow_Loaded;
		}

		private void FXComponentWindow_Loaded(object sender, RoutedEventArgs e)
		{
			var on = CreateBooleanUpDown("Graphic EQ", "on");
			eqPanel.Children.Add(on);

			var gain1 = CreateNumericUpDownControl("20Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain1");
			eqPanel.Children.Add(gain1);

			var gain2 = CreateNumericUpDownControl("25Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain2");
			eqPanel.Children.Add(gain2);

			var gain3 = CreateNumericUpDownControl("32Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain3");
			eqPanel.Children.Add(gain3);

			var gain4 = CreateNumericUpDownControl("40Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain4");
			eqPanel.Children.Add(gain4);

			var gain5 = CreateNumericUpDownControl("50Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain5");
			eqPanel.Children.Add(gain5);

			var gain6 = CreateNumericUpDownControl("63Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain6");
			eqPanel.Children.Add(gain6);

			var gain7 = CreateNumericUpDownControl("80Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain7");
			eqPanel.Children.Add(gain7);

			var gain8 = CreateNumericUpDownControl("100Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain8");
			eqPanel.Children.Add(gain8);

			var gain9 = CreateNumericUpDownControl("125Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain9");
			eqPanel.Children.Add(gain9);

			var gain10 = CreateNumericUpDownControl("160Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain10");
			eqPanel.Children.Add(gain10);

			var gain11 = CreateNumericUpDownControl("200Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain11");
			eqPanel.Children.Add(gain11);

			var gain12 = CreateNumericUpDownControl("250Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain12");
			eqPanel.Children.Add(gain12);

			var gain13 = CreateNumericUpDownControl("315Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain13");
			eqPanel.Children.Add(gain13);

			var gain14 = CreateNumericUpDownControl("400Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain14");
			eqPanel.Children.Add(gain14);

			var gain15 = CreateNumericUpDownControl("500Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain15");
			eqPanel.Children.Add(gain15);

			var gain16 = CreateNumericUpDownControl("630Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain16");
			eqPanel.Children.Add(gain16);

			var gain17 = CreateNumericUpDownControl("800Hz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain17");
			eqPanel.Children.Add(gain17);

			var gain18 = CreateNumericUpDownControl("1kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain18");
			eqPanel.Children.Add(gain18);

			var gain19 = CreateNumericUpDownControl("1.25kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain19");
			eqPanel.Children.Add(gain19);

			var gain20 = CreateNumericUpDownControl("1.6kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain20");
			eqPanel.Children.Add(gain20);

			var gain21 = CreateNumericUpDownControl("2kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain21");
			eqPanel.Children.Add(gain21);

			var gain22 = CreateNumericUpDownControl("2.5kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain22");
			eqPanel.Children.Add(gain22);

			var gain23 = CreateNumericUpDownControl("3.15kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain23");
			eqPanel.Children.Add(gain23);

			var gain24 = CreateNumericUpDownControl("4kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain24");
			eqPanel.Children.Add(gain24);

			var gain25 = CreateNumericUpDownControl("5kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain25");
			eqPanel.Children.Add(gain25);

			var gain26 = CreateNumericUpDownControl("6.3kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain26");
			eqPanel.Children.Add(gain26);

			var gain27 = CreateNumericUpDownControl("8kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain27");
			eqPanel.Children.Add(gain27);

			var gain28 = CreateNumericUpDownControl("10kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain28");
			eqPanel.Children.Add(gain28);

			var gain29 = CreateNumericUpDownControl("12.5kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain29");
			eqPanel.Children.Add(gain29);

			var gain30 = CreateNumericUpDownControl("16kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain30");
			eqPanel.Children.Add(gain30);

			var gain31 = CreateNumericUpDownControl("20kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain31");
			eqPanel.Children.Add(gain31);

			var gain32 = CreateNumericUpDownControl("20kHz", -12, 12, 0, Units.DB, CurveFormula.Linear, "gain32");
			eqPanel.Children.Add(gain32);

		}
		private NumericUpDown CreateNumericUpDownControl(string caption, float minValue, float maxValue, float def, Units unit, CurveFormula curve, string bindingPath)
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

		private BooleanUpDown CreateBooleanUpDown(string caption, string bindingPath)
		{
			var upDown = new BooleanUpDown();
			upDown.Caption = caption;
			upDown.SetBinding(BooleanUpDown.ValueProperty, new Binding(bindingPath));
			return upDown;
		}

		private ListUpDown CreateListUpDown(string caption, List<string> items, string bindingPath)
		{
			var listUpDown = new ListUpDown();
			listUpDown.Caption = caption;
			listUpDown.Items = items;
			listUpDown.SetBinding(ListUpDown.ValueProperty, new Binding(bindingPath));
			return listUpDown;
		}
	}
}

using Presonus.UCNet.Api.Models;
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

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
	/// <summary>
	/// Interaction logic for SignalGenToolWindow.xaml
	/// </summary>
	public partial class SignalGenToolWindow : ToolWindow
	{
		SignalGen signalGen;
		public SignalGenToolWindow(SignalGen signalGen)
		{
			this.signalGen = signalGen;
			DataContext = signalGen;
			InitializeComponent();
			Title = "Signal Generator";
			Loaded += SignalGenToolWindow_Loaded;
		}

		private void SignalGenToolWindow_Loaded(object sender, RoutedEventArgs e)
		{
			ControlFactory.CreateListUpDown(panel, "Signal Type", signalGen.type_values.ToList(), nameof(signalGen.type));

			ControlFactory.CreateNumericUpDownControl(panel, "Sine Frequency", 20, 20000, 440, Api.Helpers.Units.HZ, Api.Helpers.CurveFormula.Logarithmic, nameof(signalGen.freq));
		
			ControlFactory.CreateNumericUpDownControl(panel, "Level", -84, 10, 0, Api.Helpers.Units.DB, Api.Helpers.CurveFormula.LinearToVolume, nameof(signalGen.level));

		}
	}
}

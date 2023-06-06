using ControlzEx.Standard;
using MathNet.Numerics;
using Presonus.UCNet.Api;
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
	/// Interaction logic for SettingsToolWindow.xaml
	/// </summary>
	public partial class SettingsToolWindow : ToolWindow
	{
		BlindViewModel blindViewModel;
		public SettingsToolWindow(BlindViewModel blindViewModel)
		{
			this.blindViewModel = blindViewModel;
			DataContext = blindViewModel;
			InitializeComponent();
			Title = "Settings Window";
			Loaded += SettingsToolWindow_Loaded;
		}

		private void SettingsToolWindow_Loaded(object sender, RoutedEventArgs e)
		{
			ControlFactory.CreateListUpDown(
				settingsPanel,
				"Phones Cue Source",
				blindViewModel.Mastersection.phones_list_values.ToList(),
				nameof(Mastersection.phones_list))
				.DataContext = blindViewModel.Mastersection;

			ControlFactory.CreateListUpDown(
				settingsPanel,
				"Solo Mode",
				blindViewModel.Mastersection.solostyle_values,
				nameof(Mastersection.solostyle))
				.DataContext = blindViewModel.Mastersection;

			//Solo in place
			ControlFactory.CreateBooleanUpDown(
				settingsPanel,
				"Solo In Place",
				nameof(blindViewModel.Mastersection.sipOn)
				).DataContext = blindViewModel.Mastersection;

			//Solo PFL
			ControlFactory.CreateBooleanUpDown(
				settingsPanel,
				"Solo PFL",
				nameof(blindViewModel.Mastersection.soloPFL)
				).DataContext = blindViewModel.Mastersection;

			//Solo Level
			ControlFactory.CreateNumericUpDownControl(
				settingsPanel,
				"Solo Level",
				-84,
				10,
				0.735f,
				Api.Helpers.Units.DB,
				Api.Helpers.CurveFormula.LinearToVolume,
				nameof(blindViewModel.Mastersection.solo_level)
				).DataContext = blindViewModel.Mastersection;
		}
	}
}

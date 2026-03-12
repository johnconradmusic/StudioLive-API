//using MixingStation.Api;
//using MixingStation.Api.Helpers;
//using MixingStation.Api.Models;
//using MixingStation.Api.Models.Channels;
//using MixingStation.Api.ViewModels;
//using MixingStation.Wpf.Blind.UserControls;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Data;

//namespace MixingStation.Wpf.Blind.ToolWindows
//{
//	/// <summary>
//	/// Interaction logic for FXComponentWindow.xaml
//	/// </summary>
//	public partial class FXComponentWindow : ToolWindow
//	{
//		private MixerRootViewModel blindViewModel;

//		public FXComponentWindow(MixerRootViewModel viewModel)
//		{
//			//DataContext = fx;
//			blindViewModel = viewModel;
//			InitializeComponent();
//			//Title = $"FX Window - {blindViewModel.ChannelIndex} ({blindViewModel.Name})";
//			Loaded += FXComponentWindow_Loaded;
//		}

//		private void FXComponentWindow_Loaded(object sender, RoutedEventArgs e)
//		{
//			var modelChooser = new ListUpDown();
//			//modelChooser.Items = FX.FXNames.Values.ToList();
//			modelChooser.Caption = "FX Model Chooser";
//			modelChooser.SetBinding(ListUpDown.ValueProperty, new Binding("type"));
//			chooserPanel.Children.Add(modelChooser);
//			modelChooser.ValueChanged += ModelChooser_ValueChanged;
//		}

//		private void ModelChooser_ValueChanged(object sender, ListUpDownEventArgs e)
//		{
//			Task.Delay(500).Wait();
//			fxPanel.Children.Clear();
//		}

		

//		private NumericUpDown CreateNumericUpDownControl(string caption, float minValue, float maxValue, float def, Units unit, CurveFormula curve, string bindingPath)
//		{
//			var numericUpDown = new NumericUpDown();
//			numericUpDown.Caption = caption;
//			numericUpDown.Min = minValue;
//			numericUpDown.Max = maxValue;
//			numericUpDown.Unit = unit;
//			numericUpDown.Curve = curve;
//			numericUpDown.Default = def;
//			numericUpDown.SetBinding(NumericUpDown.ValueProperty, new Binding(bindingPath));
//			return numericUpDown;
//		}

//		private BooleanUpDown CreateBooleanUpDown(string caption, string bindingPath)
//		{
//			var upDown = new BooleanUpDown();
//			upDown.Caption = caption;
//			upDown.SetBinding(BooleanUpDown.ValueProperty, new Binding(bindingPath));
//			return upDown;
//		}

//		private ListUpDown CreateListUpDown(string caption, List<string> items, string bindingPath)
//		{
//			var listUpDown = new ListUpDown();
//			listUpDown.Caption = caption;
//			listUpDown.Items = items;
//			listUpDown.SetBinding(ListUpDown.ValueProperty, new Binding(bindingPath));
//			return listUpDown;
//		}
//	}
//}
using Presonus.UCNet.Api;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Wpf.Blind.UserControls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
	/// <summary>
	/// Interaction logic for FXComponentWindow.xaml
	/// </summary>
	public partial class FXComponentWindow : ToolWindow
	{
		private BlindViewModel blindViewModel;
		private FX fx;

		public FXComponentWindow(FX fxComponent, BlindViewModel viewModel)
		{
			fx = fxComponent;
			DataContext = fx;
			blindViewModel = viewModel;
			InitializeComponent();
			Title = $"FX Window - {fx.ChannelIndex} ({fx.Name})";
			Loaded += FXComponentWindow_Loaded;
		}

		private void FXComponentWindow_Loaded(object sender, RoutedEventArgs e)
		{
			var modelChooser = new ListUpDown();
			modelChooser.Items = FX.FXNames.Values.ToList();
			modelChooser.Caption = "FX Model Chooser";
			modelChooser.SetBinding(ListUpDown.ValueProperty, new Binding("type"));
			chooserPanel.Children.Add(modelChooser);
			modelChooser.ValueChanged += ModelChooser_ValueChanged;
		}

		private void ModelChooser_ValueChanged(object sender, ListUpDownEventArgs e)
		{
			fxPanel.Children.Clear();
			BuildFXControls(fx.classId);
		}

		private void BuildFXControls(string classId)
		{
			if (classId == FXClassIDs.DigitalXLReverb)
			{
				BuildDigitalXLReverb();
			}
			else if (classId == FXClassIDs.PAE16Reverb)
			{
				BuildPAE16Reverb();
			}
			else if (classId == FXClassIDs.Digital335Reverb)
			{
				BuildDigital335Reverb();
			}
			else if (classId == FXClassIDs.VintagePlateReverb)
			{
				BuildVintagePlateReverb();
			}
			else if (classId == FXClassIDs.MonoDelay)
			{
				BuildMonoDelay();
			}
			else if (classId == FXClassIDs.StereoDelay)
			{
				BuildStereoDelay();
			}
			else if (classId == FXClassIDs.PingPongDelay)
			{
				BuildPingPongDelay();
			}
			else if (classId == FXClassIDs.Chorus)
			{
				BuildChorus();
			}
			else if (classId == FXClassIDs.Flanger)
			{
				BuildFlanger();
			}
			else
			{
				// Handle unknown class ID here
			}
		}

		private void BuildFlanger()
		{
			var rate = CreateNumericUpDownControl("Rate", 0.01f, 10, 0.026f, Units.HZ, CurveFormula.Logarithmic, "rate");
			fxPanel.Children.Add(rate);

			var range = CreateNumericUpDownControl("Range", 30, 80, 0.8f, Units.PERCENT, CurveFormula.Linear, "range");
			fxPanel.Children.Add(range);

			var offset = CreateNumericUpDownControl("Offset", .35f, 3, 0, Units.MS, CurveFormula.Logarithmic, "offset");
			fxPanel.Children.Add(offset);

			var feedback = CreateNumericUpDownControl("Feedback", 30, 80, 0.8f, Units.PERCENT, CurveFormula.Linear, "feedback");
			fxPanel.Children.Add(feedback);

			var width = CreateNumericUpDownControl("Width", 0, 1, 0, Units.NONE, CurveFormula.Linear, "width");
			fxPanel.Children.Add(width);

		}

		private void BuildChorus()
		{
			var rate = CreateNumericUpDownControl("Rate", 0.01f, 10, 0.018f, Units.HZ, CurveFormula.Logarithmic, "rate");
			fxPanel.Children.Add(rate);

			var depth = CreateNumericUpDownControl("Depth", 1, 100, 0, Units.PERCENT, CurveFormula.Linear, "depth");
			fxPanel.Children.Add(depth);

			var offset = CreateNumericUpDownControl("Offset", .1f, 70, 0, Units.MS, CurveFormula.Logarithmic, "offset");
			fxPanel.Children.Add(offset);

			var feedback = CreateNumericUpDownControl("Feedback", 0, 95f, 0.13f, Units.PERCENT, CurveFormula.Linear, "feedback");
			fxPanel.Children.Add(feedback);

			var width = CreateNumericUpDownControl("Width", 0, 100, 0, Units.PERCENT, CurveFormula.Linear, "width");
			fxPanel.Children.Add(width);

			var shape = CreateNumericUpDownControl("Shape", 0, 2, 0, Units.NONE, CurveFormula.Linear, "shape");
			fxPanel.Children.Add(shape);

			var polarity = CreateBooleanUpDown("Polarity", "polarity");
			fxPanel.Children.Add(polarity);
		}

		private void BuildPingPongDelay()
		{
			var delay = CreateNumericUpDownControl("Delay", 2, 2000, 0, Units.MS, CurveFormula.Linear, "delay");
			fxPanel.Children.Add(delay);

			var dlyoffsetl = CreateNumericUpDownControl("Time Left", 2, 2000, 0, Units.MS, CurveFormula.Linear, "dly_offset_l");
			fxPanel.Children.Add(dlyoffsetl);

			var dlyoffsetr = CreateNumericUpDownControl("Time Right", 2, 2000, 0, Units.MS, CurveFormula.Linear, "dly_offset_r");
			fxPanel.Children.Add(dlyoffsetr);

			var feedback = CreateNumericUpDownControl("Feedback", 0, 100, 0, Units.PERCENT, CurveFormula.Linear, "feedback");
			fxPanel.Children.Add(feedback);

			var spread = CreateNumericUpDownControl("Spread", 0, 100, 1, Units.PERCENT, CurveFormula.Logarithmic, "spread");
			fxPanel.Children.Add(spread);

			var lpf = CreateNumericUpDownControl("Lo Pass Filter", 1000, 20000, 1, Units.HZ, CurveFormula.Logarithmic, "lpf");
			fxPanel.Children.Add(lpf);

			var hpf = CreateNumericUpDownControl("Hi Pass Filter", 20, 1000, 1, Units.HZ, CurveFormula.Logarithmic, "hpf");
			fxPanel.Children.Add(hpf);

			var fb_lpf = CreateNumericUpDownControl("Feedback Lo Pass Filter", 1000, 20000, 1, Units.HZ, CurveFormula.Logarithmic, "fb_lpf");
			fxPanel.Children.Add(fb_lpf);

			var fb_hpf = CreateNumericUpDownControl("Feedback Hi Pass Filter", 20, 1000, 1, Units.HZ, CurveFormula.Logarithmic, "fb_hpf");
			fxPanel.Children.Add(fb_hpf);
		}

		private void BuildStereoDelay()
		{
			var delayL = CreateNumericUpDownControl("Delay Left", 2, 2000, 0, Units.MS, CurveFormula.Linear, "delay_l");
			fxPanel.Children.Add(delayL);

			var delayR = CreateNumericUpDownControl("Delay Right", 2, 2000, 0, Units.MS, CurveFormula.Linear, "delay_r");
			fxPanel.Children.Add(delayR);

			var feedbackl = CreateNumericUpDownControl("Feedback", 0, 100, 0, Units.PERCENT, CurveFormula.Linear, "fb_l");
			fxPanel.Children.Add(feedbackl);

			var feedbackr = CreateNumericUpDownControl("Feedback", 0, 100, 0, Units.PERCENT, CurveFormula.Linear, "fb_r");
			fxPanel.Children.Add(feedbackr);

			var spread = CreateNumericUpDownControl("Spread", 0, 100, 1, Units.PERCENT, CurveFormula.Logarithmic, "spread");
			fxPanel.Children.Add(spread);

			var lpf = CreateNumericUpDownControl("Lo Pass Filter", 1000, 20000, 1, Units.HZ, CurveFormula.Logarithmic, "lpf");
			fxPanel.Children.Add(lpf);

			var hpf = CreateNumericUpDownControl("Hi Pass Filter", 20, 1000, 1, Units.HZ, CurveFormula.Logarithmic, "hpf");
			fxPanel.Children.Add(hpf);
		}

		private void BuildMonoDelay()
		{
			var delay = CreateNumericUpDownControl("Delay", 2, 2000, 0, Units.MS, CurveFormula.Linear, "delay");
			fxPanel.Children.Add(delay);

			var feedback = CreateNumericUpDownControl("Feedback", 0, 100, 0, Units.PERCENT, CurveFormula.Linear, "feedback");
			fxPanel.Children.Add(feedback);

			var lpf = CreateNumericUpDownControl("Lo Pass Filter", 1000, 20000, 1, Units.HZ, CurveFormula.Logarithmic, "lpf");
			fxPanel.Children.Add(lpf);

			var hpf = CreateNumericUpDownControl("Hi Pass Filter", 20, 1000, 1, Units.HZ, CurveFormula.Logarithmic, "hpf");
			fxPanel.Children.Add(hpf);

			var fb_lpf = CreateNumericUpDownControl("Feedback Lo Pass Filter", 1000, 20000, 1, Units.HZ, CurveFormula.Logarithmic, "fb_lpf");
			fxPanel.Children.Add(fb_lpf);

			var fb_hpf = CreateNumericUpDownControl("Feedback Hi Pass Filter", 20, 1000, 1, Units.HZ, CurveFormula.Logarithmic, "fb_hpf");
			fxPanel.Children.Add(fb_hpf);
		}

		private void BuildVintagePlateReverb()
		{
			var type = CreateListUpDown("Reverb Type", FX.PAE335TypeList, "verbType");
			fxPanel.Children.Add(type);


			var predelay = CreateNumericUpDownControl("Predelay", 0, 120, 0, Units.MS, CurveFormula.Linear, "predelay");
			fxPanel.Children.Add(predelay);

			var reflection = CreateNumericUpDownControl("Reflection", 0, 0.999f, 0, Units.NONE, CurveFormula.Linear, "reflection");
			fxPanel.Children.Add(reflection);

			var lpf = CreateNumericUpDownControl("Lo Pass Filter", 1000, 15000, 0, Units.HZ, CurveFormula.Logarithmic, "lpf");
			fxPanel.Children.Add(lpf);



			var lfdamp_freq = CreateNumericUpDownControl("Lo Frequency Damping Frequency", 20, 3500, 0, Units.HZ, CurveFormula.Logarithmic, "lfdamp_freq");
			fxPanel.Children.Add(lfdamp_freq);

			var lfdamp_gain = CreateNumericUpDownControl("Lo Frequency Damping Gain", -12, 0, 0, Units.DB, CurveFormula.Linear, "lfdamp_gain");
			fxPanel.Children.Add(lfdamp_gain);
		}

		private void BuildDigital335Reverb()
		{
			var type = CreateListUpDown("Reverb Type", FX.PAE335TypeList, "verbType");
			fxPanel.Children.Add(type);

			var predelay = CreateNumericUpDownControl("Predelay", 0, 120, 0, Units.MS, CurveFormula.Linear, "predelay");
			fxPanel.Children.Add(predelay);

			var diffusion = CreateNumericUpDownControl("Diffusion", 0, .9f, 0, Units.NONE, CurveFormula.Linear, "diffusion");
			fxPanel.Children.Add(diffusion);

			var reflection = CreateNumericUpDownControl("Reflection", 0, 0.999f, 0, Units.NONE, CurveFormula.Linear, "reflection");
			fxPanel.Children.Add(reflection);

			var lpf = CreateNumericUpDownControl("Lo Pass Filter", 1000, 15000, 0, Units.HZ, CurveFormula.Logarithmic, "lpf");
			fxPanel.Children.Add(lpf);

			var size = CreateNumericUpDownControl("Hall Size", 1, 50, 0, Units.NONE, CurveFormula.Linear, "size");
			fxPanel.Children.Add(size);

			var lfdamp_freq = CreateNumericUpDownControl("Lo Frequency Damping Frequency", 20, 3500, 0, Units.HZ, CurveFormula.Logarithmic, "lfdamp_freq");
			fxPanel.Children.Add(lfdamp_freq);

			var lfdamp_gain = CreateNumericUpDownControl("Lo Frequency Damping Gain", -12, 0, 0, Units.DB, CurveFormula.Linear, "lfdamp_gain");
			fxPanel.Children.Add(lfdamp_gain);
		}

		private void BuildPAE16Reverb()
		{
			var type = CreateListUpDown("Reverb Type", FX.PAE16TypeList, "verbType");
			fxPanel.Children.Add(type);

			var predelay = CreateNumericUpDownControl("Predelay", 0, 120, 0, Units.MS, CurveFormula.Linear, "predelay");
			fxPanel.Children.Add(predelay);

			var diffusion = CreateNumericUpDownControl("Diffusion", 0, .9f, 0, Units.NONE, CurveFormula.Linear, "diffusion");
			fxPanel.Children.Add(diffusion);

			var reflection = CreateNumericUpDownControl("Reflection", 0, 0.999f, 0, Units.NONE, CurveFormula.Linear, "reflection");
			fxPanel.Children.Add(reflection);

			var lpf = CreateNumericUpDownControl("Lo Pass Filter", 1000, 15000, 0, Units.HZ, CurveFormula.Logarithmic, "lpf");
			fxPanel.Children.Add(lpf);

			var lfdamp_freq = CreateNumericUpDownControl("Lo Frequency Damping Frequency", 20, 3500, 0, Units.HZ, CurveFormula.Logarithmic, "lfdamp_freq");
			fxPanel.Children.Add(lfdamp_freq);

			var lfdamp_gain = CreateNumericUpDownControl("Lo Frequency Damping Gain", -12, 0, 0, Units.DB, CurveFormula.Linear, "lfdamp_gain");
			fxPanel.Children.Add(lfdamp_gain);
		}

		private void BuildDigitalXLReverb()
		{
			var type = CreateListUpDown("Reverb Type", FX.DigitalXLReverbTypeList, "verbType");
			fxPanel.Children.Add(type);

			var predelay = CreateNumericUpDownControl("Predelay", 0, 120, 0, Units.MS, CurveFormula.Linear, "predelay");
			fxPanel.Children.Add(predelay);

			var reflection = CreateNumericUpDownControl("Reflection", 0, 0.999f, 0, Units.NONE, CurveFormula.Linear, "reflection");
			fxPanel.Children.Add(reflection);

			var size = CreateNumericUpDownControl("Hall Size", 1, 50, 0, Units.NONE, CurveFormula.Linear, "size");
			fxPanel.Children.Add(size);

			var lpf = CreateNumericUpDownControl("Lo Pass Filter", 1000, 15000, 0, Units.HZ, CurveFormula.Logarithmic, "lpf");
			fxPanel.Children.Add(lpf);

			var lfdamp_freq = CreateNumericUpDownControl("Lo Frequency Damping Frequency", 20, 3500, 0, Units.HZ, CurveFormula.Logarithmic, "lfdamp_freq");
			fxPanel.Children.Add(lfdamp_freq);

			var lfdamp_gain = CreateNumericUpDownControl("Lo Frequency Damping Gain", -12, 0, 0, Units.DB, CurveFormula.Linear, "lfdamp_gain");
			fxPanel.Children.Add(lfdamp_gain);
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
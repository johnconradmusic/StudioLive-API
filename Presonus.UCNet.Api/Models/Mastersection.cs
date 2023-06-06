using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Presonus.UCNet.Api.Models
{
	public class Mastersection : ParameterRouter
	{
		public Mastersection(MixerStateService mixerStateService) : base("mastersection", -1, mixerStateService)
		{

		}

		//Solo
		public bool soloPFL { get => GetBoolean(); set => SetBoolean(value); }
		public bool sipOn { get => GetBoolean(); set => SetBoolean(value); }
		public bool anysolo { get => GetBoolean(); set => SetBoolean(value); }
		public bool solo_selects { get => GetBoolean(); set => SetBoolean(value); }
		public float solostyle { get => GetValue(); set => SetValue(value); }
		public List<string> solostyle_values = new() { "Latch", "Radio", "CR" };
		public float solo_level { get => GetValue(); set => SetValue(value); }

		//Monitor
		public float mon_list { get => GetValue(); set => SetValue(value); }
		public string[] mon_list_values { get => GetStrings("mastersection/mon_list"); }
		public float mon_level { get => GetValue(); set => SetValue(value); }
		public float mon_delay { get => GetValue(); set => SetValue(value); }


		//Phones
		public float phones_list { get => GetValue(); set => SetValue(value); }
		public string[] phones_list_values { get => GetStrings("mastersection/phones_list"); }
		public float phones_level { get => GetValue(); set => SetValue(value); }
		public float phones_delay { get => GetValue(); set => SetValue(value); }

		//Talkback
		public bool talkpressed { get => GetBoolean(); set => SetBoolean(value); }


		public override event PropertyChangedEventHandler PropertyChanged;
		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}

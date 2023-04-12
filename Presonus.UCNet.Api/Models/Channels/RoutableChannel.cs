using Presonus.UCNet.Api.Enums;
using Presonus.UCNet.Api.NewDataModel;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Channels
{
	public class RoutableChannel : Channel
	{
		public RoutableChannel(ChannelTypes channelType, int index, MixerStateService mixerStateService, MeterDataStorage meterDataStorage) : base(channelType, index, mixerStateService, meterDataStorage)
		{

		}

		public bool assign_aux1 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux2 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux3 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux4 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux5 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux6 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux7 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux8 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux9 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux10 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux11 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux12 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux13 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux14 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux15 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux16 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux17 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux18 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux19 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux20 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux21 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux22 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux23 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux24 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux25 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux26 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux27 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux28 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux29 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux30 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux31 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_aux32 { get => GetBoolean(); set => SetBoolean(value); }

		public float aux1 { get => GetValue(); set => SetValue(value); }
		public float aux2 { get => GetValue(); set => SetValue(value); }
		public float aux3 { get => GetValue(); set => SetValue(value); }
		public float aux4 { get => GetValue(); set => SetValue(value); }
		public float aux5 { get => GetValue(); set => SetValue(value); }
		public float aux6 { get => GetValue(); set => SetValue(value); }
		public float aux7 { get => GetValue(); set => SetValue(value); }
		public float aux8 { get => GetValue(); set => SetValue(value); }
		public float aux9 { get => GetValue(); set => SetValue(value); }
		public float aux10 { get => GetValue(); set => SetValue(value); }
		public float aux11 { get => GetValue(); set => SetValue(value); }
		public float aux12 { get => GetValue(); set => SetValue(value); }
		public float aux13 { get => GetValue(); set => SetValue(value); }
		public float aux14 { get => GetValue(); set => SetValue(value); }
		public float aux15 { get => GetValue(); set => SetValue(value); }
		public float aux16 { get => GetValue(); set => SetValue(value); }
		public float aux17 { get => GetValue(); set => SetValue(value); }
		public float aux18 { get => GetValue(); set => SetValue(value); }
		public float aux19 { get => GetValue(); set => SetValue(value); }
		public float aux20 { get => GetValue(); set => SetValue(value); }
		public float aux21 { get => GetValue(); set => SetValue(value); }
		public float aux22 { get => GetValue(); set => SetValue(value); }
		public float aux23 { get => GetValue(); set => SetValue(value); }
		public float aux24 { get => GetValue(); set => SetValue(value); }
		public float aux25 { get => GetValue(); set => SetValue(value); }
		public float aux26 { get => GetValue(); set => SetValue(value); }
		public float aux27 { get => GetValue(); set => SetValue(value); }
		public float aux28 { get => GetValue(); set => SetValue(value); }
		public float aux29 { get => GetValue(); set => SetValue(value); }
		public float aux30 { get => GetValue(); set => SetValue(value); }
		public float aux31 { get => GetValue(); set => SetValue(value); }
		public float aux32 { get => GetValue(); set => SetValue(value); }

		public float aux12_pan { get => GetValue(); set => SetValue(value); }
		public float aux34_pan { get => GetValue(); set => SetValue(value); }
		public float aux56_pan { get => GetValue(); set => SetValue(value); }
		public float aux78_pan { get => GetValue(); set => SetValue(value); }
		public float aux910_pan { get => GetValue(); set => SetValue(value); }
		public float aux1112_pan { get => GetValue(); set => SetValue(value); }
		public float aux1314_pan { get => GetValue(); set => SetValue(value); }
		public float aux1516_pan { get => GetValue(); set => SetValue(value); }
		public float aux1718_pan { get => GetValue(); set => SetValue(value); }
		public float aux1920_pan { get => GetValue(); set => SetValue(value); }
		public float aux2122_pan { get => GetValue(); set => SetValue(value); }
		public float aux2324_pan { get => GetValue(); set => SetValue(value); }
		public float aux2526_pan { get => GetValue(); set => SetValue(value); }
		public float aux2728_pan { get => GetValue(); set => SetValue(value); }
		public float aux2930_pan { get => GetValue(); set => SetValue(value); }
		public float aux3132_pan { get => GetValue(); set => SetValue(value); }

		public float aux12_stpan { get => GetValue(); set => SetValue(value); }
		public float aux34_stpan { get => GetValue(); set => SetValue(value); }
		public float aux56_stpan { get => GetValue(); set => SetValue(value); }
		public float aux78_stpan { get => GetValue(); set => SetValue(value); }
		public float aux910_stpan { get => GetValue(); set => SetValue(value); }
		public float aux1112_stpan { get => GetValue(); set => SetValue(value); }
		public float aux1314_stpan { get => GetValue(); set => SetValue(value); }
		public float aux1516_stpan { get => GetValue(); set => SetValue(value); }
		public float aux1718_stpan { get => GetValue(); set => SetValue(value); }
		public float aux1920_stpan { get => GetValue(); set => SetValue(value); }
		public float aux2122_stpan { get => GetValue(); set => SetValue(value); }
		public float aux2324_stpan { get => GetValue(); set => SetValue(value); }
		public float aux2526_stpan { get => GetValue(); set => SetValue(value); }
		public float aux2728_stpan { get => GetValue(); set => SetValue(value); }
		public float aux2930_stpan { get => GetValue(); set => SetValue(value); }
		public float aux3132_stpan { get => GetValue(); set => SetValue(value); }

		public bool mono { get => GetBoolean(); set => SetBoolean(value); }
		public float monolevel { get => GetValue(); set => SetValue(value); }
		public float centerdiv { get => GetValue(); set => SetValue(value); }

		public List<string> adc_src_values => GetValueList(nameof(adc_src));
		public List<string> avb_src_values => GetValueList(nameof(avb_src));
		public List<string> usb_src_values => GetValueList(nameof(usb_src));
		public List<string> sd_src_values => GetValueList(nameof(sd_src));

		public float adc_src { get => GetValue(); set => SetValue(value); }
		public float avb_src { get => GetValue(); set => SetValue(value); }
		public float usb_src { get => GetValue(); set => SetValue(value); }
		public float sd_src { get => GetValue(); set => SetValue(value); }

		public float adc_src2 { get => GetValue(); set => SetValue(value); }
		public float avb_src2 { get => GetValue(); set => SetValue(value); }
		public float usb_src2 { get => GetValue(); set => SetValue(value); }
		public float sd_src2 { get => GetValue(); set => SetValue(value); }
	}
}

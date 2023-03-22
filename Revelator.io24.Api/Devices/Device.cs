﻿using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Auxes;
using Presonus.StudioLive32.Api.Models.Inputs;
using Presonus.StudioLive32.Api.Models.Outputs;
using Presonus.UCNet.Api.Sound;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Presonus.StudioLive32.Api;
using Presonus.UCNet.Api.Models.Global;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Services;

namespace Presonus.UCNet.Api.Devices
{
	public abstract class Device : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs) { PropertyChanged?.Invoke(this, eventArgs); }

		public bool AutoClipAvoidance { get; set; } = true;
		public bool IsAnyChannelClipping
		{
			get
			{
				return Channels.Any((c) => c.meter > -3);
			}
		}


		public MixerStateService _rawService;
		public Mastersection Mastersection { get; set; }
		public Global Global { get; set; }
		public Models.GEQ GEQ { get; set; }
		public List<ChannelBase> Channels { get; set; } = new List<ChannelBase>();
		public List<BusChannel> Buses { get; set; } = new List<BusChannel>();
		public Device(MixerStateService rawService, int lineChannels, int returnChannels, int auxChannels, int fxReturns)
		{

			_rawService = rawService;

			for (int i = 0; i < lineChannels; i++)
			{
				var chan = new LineChannel("line/ch" + (i + 1).ToString(), rawService, this);
				Channels.Add(chan);
			}
			for (int i = 0; i < fxReturns; i++)
			{
				var chan = new ReturnChannel("fxreturn/ch" + (i + 1).ToString(), rawService, this);
				Channels.Add(chan);
			}
			for (int i = 0; i < returnChannels; i++)
			{
				var chan = new ReturnChannel("return/ch" + (i + 1).ToString(), rawService, this);
				Channels.Add(chan);
			}
			for (int i = 0; i < auxChannels; i++)
			{
				var chan = new BusChannel("aux/ch" + (i + 1).ToString(), rawService, this);
				Channels.Add(chan);
				Buses.Add(chan);
			}
			var main = new BusChannel("main/ch1", rawService, this);
			Channels.Add(main);
			Buses.Add(main);

			GEQ = new Models.GEQ("geq/ch7", rawService);

			Global = new Global("global", rawService);

			Mastersection = new Mastersection("mastersection", rawService);
		}


		private void Chan_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "pan" || e.PropertyName == "stereopan")
			{
				OnPropertyChanged(new PropertyChangedEventArgs("DynamicPan"));
				return;
			}

			OnPropertyChanged(e);
		}
	}
}

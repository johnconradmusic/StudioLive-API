using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Models.Global;
using Revelator.io24.Api.Models.Inputs;
using Revelator.io24.Api.Models.Outputs;
using System.Collections.Generic;
using System.ComponentModel;

namespace Revelator.io24.Api
{
	public class Device : INotifyPropertyChanged
	{

		private readonly RawService _rawService;
		public RawService RawService => _rawService;
		public Global Global { get; }

		public List<LineChannel> Channels { get; set; } = new List<LineChannel>();

		//public LineChannel Channel1 { get; }

		public readonly int ChannelCount = 32;

        public event PropertyChangedEventHandler PropertyChanged;

        public Main Main { get; }
		public Device(RawService rawService)
		{
			_rawService = rawService;

			Global = new Global(rawService);
			for (int i = 0; i < ChannelCount; i++)
			{
				var chan = new LineChannel("line/ch" + (i + 1).ToString(), rawService);
				Channels.Add(chan);
				chan.PropertyChanged += PropertyChanged;
			}

			Main = new Main(rawService);

		}
	}
}

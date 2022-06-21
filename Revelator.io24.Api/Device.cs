using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Models.Auxes;
using Revelator.io24.Api.Models.Global;
using Revelator.io24.Api.Models.Inputs;
using Revelator.io24.Api.Models.Outputs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Revelator.io24.Api
{
    public class Device 
    {

        private readonly RawService _rawService;
        public RawService RawService => _rawService;
        public Global Global { get; }

        public ObservableCollection<LineChannel> Channels { get; set; } = new ObservableCollection<LineChannel>();

        public ObservableCollection<AuxChannel> AuxChannels { get; set; } = new ObservableCollection<AuxChannel>();

        public readonly int ChannelCount = 32;
        public readonly int AuxCount = 16;

        public Main Main { get; }
        public Device(RawService rawService)
        {
            _rawService = rawService;

            Global = new Global(rawService);
            for (int i = 0; i < ChannelCount; i++)
            {
                var chan = new LineChannel("line/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
            }
            for (int i = 0; i < AuxCount; i++)
            {
                var chan = new AuxChannel("aux/ch" + (i + 1).ToString(), rawService);
                AuxChannels.Add(chan);
            }
            Main = new Main(rawService);

        }

  
        
    }
}

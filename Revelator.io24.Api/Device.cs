using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Models;
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
        public List<ChannelBase> Channels { get; set; } = new List<ChannelBase>();
        public List<BusChannel> Buses { get; set; } = new List<BusChannel>();

        public readonly int ChannelCount = 32;
        public readonly int ReturnCount = 3;
        public readonly int AuxCount = 16;
        public readonly int FXReturnCount = 4;
        public Device(RawService rawService)
        {
            _rawService = rawService;

            Global = new Global(rawService);
            for (int i = 0; i < ChannelCount; i++)
            {
                var chan = new LineChannel("line/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
            }
            for (int i = 0; i < FXReturnCount; i++)
            {
                var chan = new ReturnChannel("fxreturn/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
            }
            for (int i = 0; i < ReturnCount; i++)
            {
                var chan = new ReturnChannel("return/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
            }
            for (int i = 0; i < AuxCount; i++)
            {
                var chan = new BusChannel("aux/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
                Buses.Add(chan);
            }
            var main = new BusChannel("main/ch1", rawService);

            Channels.Add(main);
            Buses.Add(main);

        }



    }
}

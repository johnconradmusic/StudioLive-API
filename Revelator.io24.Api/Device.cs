using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Auxes;
using Presonus.StudioLive32.Api.Models.Inputs;
using Presonus.StudioLive32.Api.Models.Outputs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;

namespace Presonus.StudioLive32.Api
{
    public class Device : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs) { PropertyChanged?.Invoke(this, eventArgs); }

        public bool AutoClipAvoidance { get; set; } = true;

        public DateTime timeOfLastClipCheck = DateTime.Now;



        public bool IsAnyChannelClipping
        {
            get
            {
                return Channels.Any((c) => c.level_meter > -1);
            }
        }

        private readonly RawService _rawService;
        public RawService RawService => _rawService;
        public List<ChannelBase> Channels { get; set; } = new List<ChannelBase>();
        public List<BusChannel> Buses { get; set; } = new List<BusChannel>();
        public readonly int ChannelCount = 32;
        public readonly int ReturnCount = 3;
        public readonly int AuxCount = 16;
        public readonly int FXReturnCount = 4;
        public Device(RawService rawService)
        {


            _rawService = rawService;

            for (int i = 0; i < ChannelCount; i++)
            {
                var chan = new LineChannel("line/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
                chan.PropertyChanged += Chan_PropertyChanged;
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
            //start timer for clip check


        }


        private void CheckClips()
        {
            if (!AutoClipAvoidance) return;
            Console.WriteLine("check clips");
            if (IsAnyChannelClipping)
            {
                foreach (var chan in Channels)
                {
                    if (chan.level_meter > -1)
                    {
                        if (chan is LineChannel lineChannel)
                        {

                            Console.WriteLine("hndling clip : old value is " + lineChannel.preampgain);
                            lineChannel.preampgain -= 3;

                        }
                    }
                }
            }
        }

        private void Chan_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "level_meter")
            {
                if (IsAnyChannelClipping)
                {
                    CheckClips();
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAnyChannelClipping)));
            }
            OnPropertyChanged(e);
        }
    }
}

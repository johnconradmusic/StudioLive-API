using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Models.Global;
using Revelator.io24.Api.Models.Inputs;
using Revelator.io24.Api.Models.Outputs;

namespace Revelator.io24.Api
{
    public class Device
    {
        private readonly RawService _rawService;
        public RawService RawService => _rawService;
        public Global Global { get; }

        public LineChannel Channel1 { get; }
        public LineChannel Channel2 { get; }


        //public LineChannel Channel1 { get; }

        public Playback Playback { get; }
        public VirtualA VirtualA { get; }
        public VirtualB VirtualB { get; }

        public Reverb Reverb { get; }

        public Main Main { get; }
        public StreamMixA StreamMixA { get; }
        public StreamMixB StreamMixB { get; }

        public Device(RawService rawService)
        {
            _rawService = rawService;

            Global = new Global(rawService);

            Channel1 = new LineChannel("line/ch1", rawService);
            Channel2 = new LineChannel("line/ch2", rawService);

            Playback = new Playback(rawService);
            VirtualA = new VirtualA(rawService);
            VirtualB = new VirtualB(rawService);

            Reverb = new Reverb(rawService);

            Main = new Main(rawService);
            StreamMixA = new StreamMixA(rawService);
            StreamMixB = new StreamMixB(rawService);
        }
    }
}

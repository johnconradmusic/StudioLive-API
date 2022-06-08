using Revelator.io24.Api.Attributes;
using System;

namespace Revelator.io24.Api.Models.Inputs
{
    public class LineChannel : InputChannel
    {
        private readonly RawService _rawService;
        public LineChannel(string routingPrefix, RawService rawService)
            : base(routingPrefix, rawService)
        {
            _rawService = rawService;
        }

        [RouteValue("clip")]
        public bool Clip
        {
            get => GetBoolean();
        }

        [RouteValue("preampgain")]
        public int Trim
        {
            get => GetVolume();
            set => SetVolume(value);
        }

        [RouteValue("dca/volume")]
        public int Volume
        {
            get => GetVolume();
            set => SetVolume(value);
        }

        [RouteValue("48v")]
        public bool PhantomPower
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

    }
}

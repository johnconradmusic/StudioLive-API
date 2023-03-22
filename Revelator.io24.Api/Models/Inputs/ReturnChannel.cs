using Newtonsoft.Json;
using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models.Inputs;
using Presonus.UCNet.Api.Devices;
using Presonus.UCNet.Api.Enums;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.StudioLive32.Api.Models.Inputs
{
    public class ReturnChannel : InputChannel
    {
        public ReturnChannel(string routingPrefix, MixerStateService rawService, Device device) : base(routingPrefix, rawService, device) { }

        [RouteValueRange(0, 20, Enums.Unit.db)]
        public float trim { get => GetValue(); set => SetValue(value); }
        [RouteValue("inputsrc")]
        public ReturnInputSource returninputsrc { get => GetEnumValue<ReturnInputSource>(); set => SetEnumValue(value); }

    }
}

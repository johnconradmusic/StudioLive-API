using Newtonsoft.Json;
using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models.Inputs;
using Presonus.UC.Api.Devices;
using Presonus.UC.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.StudioLive32.Api.Models.Inputs
{
    public class ReturnChannel : InputChannel
    {
        public ReturnChannel(string routingPrefix, RawService rawService, Device device) : base(routingPrefix, rawService, device) { }

        [RouteValueRange(0, 20, Enums.Unit.db)]
        public float trim { get => GetValue(); set => SetValue(value); }
        [RouteValue("inputsrc")]
        public ReturnInputSource returninputsrc { get => GetEnumValue<ReturnInputSource>(); set => SetEnumValue(value); }

    }
}

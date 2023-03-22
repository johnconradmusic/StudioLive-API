using Presonus.StudioLive32.Api;
using Presonus.UCNet.Api.Models.Global;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Devices
{
    public class Studio1824C : Device
    {
        public Studio1824C(MixerStateService rawService) : base(rawService, 36, 0, 7, 0)
        {

        }
    }
}

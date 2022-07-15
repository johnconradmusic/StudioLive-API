using Presonus.StudioLive32.Api;
using Presonus.UC.Api.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Devices
{
    public class Studio1824C : Device
    {
        public Studio1824C(RawService rawService) : base(rawService, 36, 0, 7, 0)
        {

        }
    }
}

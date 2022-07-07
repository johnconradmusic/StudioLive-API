using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Enums
{
    public enum BusChannelRoutingMode { PreFader, PostFX, PostFader }
    public enum DawPostDsp { Pre, Post }
    public enum LineInputSource { Analog, Network, USB, SD }
    public enum ReturnInputSource { Network, USB, SD}
    public enum BusSource { Mixer, Network }
}

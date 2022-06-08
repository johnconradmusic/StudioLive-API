using Revelator.io24.Api.Attributes;

namespace Revelator.io24.Api.Models.Inputs
{
    public class Playback : InputChannel
    {
        public Playback(RawService rawService)
            : base("return/ch1", rawService)
        {
            //
        }
    }
}

using Revelator.io24.Api.Attributes;

namespace Revelator.io24.Api.Models.Outputs
{
    public class StreamMixA : OutputChannel
    {
        public StreamMixA(RawService rawService)
            : base("aux/ch1", rawService)
        {
            //
        }
    }
}

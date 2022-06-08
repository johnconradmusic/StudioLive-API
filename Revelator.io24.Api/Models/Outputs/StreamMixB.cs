using Revelator.io24.Api.Attributes;

namespace Revelator.io24.Api.Models.Outputs
{
    public class StreamMixB : OutputChannel
    {
        public StreamMixB(RawService rawService)
            : base("aux/ch2", rawService)
        {
            //
        }
    }
}

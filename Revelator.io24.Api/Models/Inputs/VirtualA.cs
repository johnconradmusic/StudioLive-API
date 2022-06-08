using Revelator.io24.Api.Attributes;

namespace Revelator.io24.Api.Models.Inputs
{
    public class VirtualA : InputChannel
    {
        public VirtualA(RawService rawService)
            : base("return/ch2", rawService)
        {
            //
        }
    }
}

using Revelator.io24.Api.Attributes;

namespace Revelator.io24.Api.Models.Inputs
{
    public class VirtualB : InputChannel
    {
        public VirtualB(RawService rawService)
            : base("return/ch3", rawService)
        {
            //
        }
    }
}

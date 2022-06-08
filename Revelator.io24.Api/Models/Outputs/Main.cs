using Revelator.io24.Api.Attributes;

namespace Revelator.io24.Api.Models.Outputs
{
    public class Main : OutputChannel
    {
        public Main(RawService rawService)
            : base("main/ch1", rawService)
        {
            //
        }

        [RouteValue("hardwareMute")]
        public bool HardwareMute
        {
            get => GetBoolean();
            //set => SetBoolean(value);
        }
    }
}

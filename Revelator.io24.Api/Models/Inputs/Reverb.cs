using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Models.Effects;

namespace Revelator.io24.Api.Models.Inputs
{
    public class Reverb : InputChannel
    {
        public Reverb(RawService rawService)
            : base("fxreturn/ch1", rawService)
        {
            Effects = new ReverbEffects("fx/ch1/reverb", rawService);
        }

        public ReverbEffects Effects { get; }
    }
}

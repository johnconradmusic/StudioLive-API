using Revelator.io24.Api.Attributes;

namespace Revelator.io24.Api.Models
{
    public abstract class OutputChannel : ChannelBase
    {
        protected OutputChannel(string routePrefix, RawService rawService)
            : base(routePrefix, rawService)
        {
            //
        }

        [RouteValue("mono")]
        public bool Mono
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        [RouteValue("clip")]
        public bool Clip
        {
            get => GetBoolean();
        }
    }
}

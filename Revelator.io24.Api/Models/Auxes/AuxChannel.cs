using Revelator.io24.Api.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revelator.io24.Api.Models.Auxes
{
    public class AuxChannel : DeviceRoutingBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string AutomationName => username;
        public string AutomationId => _routePrefix + username;
        public bool LinkSlave => !(link && !linkmaster);

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }
        public AuxChannel(string routePrefix, RawService rawService) : base(routePrefix, rawService)
        {

        }
        public string username
        {
            get => GetString();
            set => SetString(value);
        }
        public int color
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public bool solo
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float volume
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }
        public bool mute
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
        public float pan { get => GetValue(); set => SetValue(value); }
        public float stereopan { get => GetValue(); set => SetValue(value); }
        public bool link
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
        public bool linkmaster
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
    }
}

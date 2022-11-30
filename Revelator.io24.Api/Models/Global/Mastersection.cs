using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Models;
using Presonus.UC.Api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Models.Global
{
    public class Mastersection : DeviceRoutingBase
    {
        public Mastersection(string routePrefix, RawService rawService) : base(routePrefix, rawService)
        {
        }

        public HeadphoneSource phones_list { get => GetEnumValue<HeadphoneSource>(); set => SetEnumValue(value); }

        public override event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}

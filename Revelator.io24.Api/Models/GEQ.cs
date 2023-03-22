using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presonus.StudioLive32.Api.Enums;
using Newtonsoft.Json;
using Presonus.UCNet.Api.Services;

namespace Presonus.UCNet.Api.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class GEQ : DeviceRoutingBase, INotifyPropertyChanged
    {
        public GEQ(string routePrefix, MixerStateService rawService) : base(routePrefix, rawService)
        {

        }

        [RouteValueRange(-15, 15, Unit.db)] public float gain1 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain2 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain3 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain4 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain5 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain6 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain7 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain8 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain9 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain10 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain11 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain12 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain13 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain14 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain15 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain16 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain17 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain18 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain19 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain20 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain21 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain22 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain23 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain24 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain25 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain26 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain27 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain28 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain29 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain30 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Unit.db)] public float gain31 { get => GetValue(); set => SetValue(value); }


        public override event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            //Console.WriteLine(eventArgs.PropertyName + " CHANGED?");
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}

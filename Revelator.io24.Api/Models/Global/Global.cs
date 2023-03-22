using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Global
{
    public class Global : DeviceRoutingBase, INotifyPropertyChanged
    {

        public Global(string routingPrefix, MixerStateService rawService) : base(routingPrefix, rawService)
        {
            Console.WriteLine("global created");
        }


        [RouteValue("mute")]
        public bool globalmute { get => GetBoolean(); set => SetBoolean(value); }

        [RouteValue("mono")]
        public bool globalmono { get => GetBoolean(); set => SetBoolean(value); }

        public bool mic_line { get => GetBoolean(); set => SetBoolean(value); }

        public bool aux1_mirror_main { get => GetBoolean(); set => SetBoolean(value); }

        public bool aux2_mirror_main { get => GetBoolean(); set => SetBoolean(value); }

        public bool aux3_mirror_main { get => GetBoolean(); set => SetBoolean(value); }

        public bool aux4_mirror_main { get => GetBoolean(); set => SetBoolean(value); }

        public bool aux5_mirror_main { get => GetBoolean(); set => SetBoolean(value); }

        public bool aux6_mirror_main { get => GetBoolean(); set => SetBoolean(value); }

        public bool aux7_mirror_main { get => GetBoolean(); set => SetBoolean(value); }

        [RouteValue("48v")]
        public bool phantom
        {
            get
            {
                //Console.WriteLine("get phantom");
                return GetBoolean();
            }
            set
            {
                //Console.WriteLine("SET PHANTOM ");
                SetBoolean(value);
            }
        }
        public override event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            //Console.WriteLine(eventArgs.PropertyName + " CHANGED?");
            PropertyChanged?.Invoke(this, eventArgs);
        }


    }
}

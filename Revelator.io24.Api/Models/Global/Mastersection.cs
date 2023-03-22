﻿using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Models;
using Presonus.UCNet.Api.Enums;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Global
{
    public class Mastersection : DeviceRoutingBase
    {
        public Mastersection(string routePrefix, MixerStateService rawService) : base(routePrefix, rawService)
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

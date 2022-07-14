using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Auxes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presonus.StudioLive32.Wpf.Views
{
    public class SendsOnFadersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public BusChannel Channel { get; set; }
        public Device device { get; set; }
        public SendsOnFadersViewModel(ChannelBase channel, Device device)
        {
            if (channel is BusChannel bus)
                Channel = bus;

            this.device = device;
        }

        //have an aux (aux1) need to get that property on each linechannel

        string sendName => "aux" + (device.Buses.IndexOf(Channel) + 1).ToString();

        string stereoGroupName
        {
            get
            {
                var thisBusNum = device.Buses.IndexOf(Channel) + 1;
                if (thisBusNum % 2 == 0)
                {  //even
                    return "aux" + (thisBusNum - 1).ToString() + thisBusNum.ToString() + "_pan";
                }
                else //odd
                {
                    return "aux" + thisBusNum.ToString() + (thisBusNum + 1).ToString() + "_pan";
                }
            }
        }

        PropertyInfo auxLevelProp => typeof(ChannelBase).GetProperty(sendName);
        PropertyInfo auxPanProp => typeof(ChannelBase).GetProperty(stereoGroupName);



        public float Ch1 { get => (float)auxLevelProp.GetValue(device.Channels[0]); set => auxLevelProp.SetValue(device.Channels[0], value); }
        public float Ch2 { get => (float)auxLevelProp.GetValue(device.Channels[1]); set => auxLevelProp.SetValue(device.Channels[1], value); }
        public float Ch3 { get => (float)auxLevelProp.GetValue(device.Channels[2]); set => auxLevelProp.SetValue(device.Channels[2], value); }
        public float Ch4 { get => (float)auxLevelProp.GetValue(device.Channels[3]); set => auxLevelProp.SetValue(device.Channels[3], value); }
        public float Ch5 { get => (float)auxLevelProp.GetValue(device.Channels[4]); set => auxLevelProp.SetValue(device.Channels[4], value); }
        public float Ch6 { get => (float)auxLevelProp.GetValue(device.Channels[5]); set => auxLevelProp.SetValue(device.Channels[5], value); }
        public float Ch7 { get => (float)auxLevelProp.GetValue(device.Channels[6]); set => auxLevelProp.SetValue(device.Channels[6], value); }
        public float Ch8 { get => (float)auxLevelProp.GetValue(device.Channels[7]); set => auxLevelProp.SetValue(device.Channels[7], value); }
        public float Ch9 { get => (float)auxLevelProp.GetValue(device.Channels[8]); set => auxLevelProp.SetValue(device.Channels[8], value); }
        public float Ch10 { get => (float)auxLevelProp.GetValue(device.Channels[9]); set => auxLevelProp.SetValue(device.Channels[9], value); }
        public float Ch11 { get => (float)auxLevelProp.GetValue(device.Channels[10]); set => auxLevelProp.SetValue(device.Channels[10], value); }
        public float Ch12 { get => (float)auxLevelProp.GetValue(device.Channels[11]); set => auxLevelProp.SetValue(device.Channels[11], value); }
        public float Ch13 { get => (float)auxLevelProp.GetValue(device.Channels[12]); set => auxLevelProp.SetValue(device.Channels[12], value); }
        public float Ch14 { get => (float)auxLevelProp.GetValue(device.Channels[13]); set => auxLevelProp.SetValue(device.Channels[13], value); }
        public float Ch15 { get => (float)auxLevelProp.GetValue(device.Channels[14]); set => auxLevelProp.SetValue(device.Channels[14], value); }
        public float Ch16 { get => (float)auxLevelProp.GetValue(device.Channels[15]); set => auxLevelProp.SetValue(device.Channels[15], value); }
        public float Ch17 { get => (float)auxLevelProp.GetValue(device.Channels[16]); set => auxLevelProp.SetValue(device.Channels[16], value); }
        public float Ch18 { get => (float)auxLevelProp.GetValue(device.Channels[17]); set => auxLevelProp.SetValue(device.Channels[17], value); }
        public float Ch19 { get => (float)auxLevelProp.GetValue(device.Channels[18]); set => auxLevelProp.SetValue(device.Channels[18], value); }
        public float Ch20 { get => (float)auxLevelProp.GetValue(device.Channels[19]); set => auxLevelProp.SetValue(device.Channels[19], value); }
        public float Ch21 { get => (float)auxLevelProp.GetValue(device.Channels[20]); set => auxLevelProp.SetValue(device.Channels[20], value); }
        public float Ch22 { get => (float)auxLevelProp.GetValue(device.Channels[21]); set => auxLevelProp.SetValue(device.Channels[21], value); }
        public float Ch23 { get => (float)auxLevelProp.GetValue(device.Channels[22]); set => auxLevelProp.SetValue(device.Channels[22], value); }
        public float Ch24 { get => (float)auxLevelProp.GetValue(device.Channels[23]); set => auxLevelProp.SetValue(device.Channels[23], value); }
        public float Ch25 { get => (float)auxLevelProp.GetValue(device.Channels[24]); set => auxLevelProp.SetValue(device.Channels[24], value); }
        public float Ch26 { get => (float)auxLevelProp.GetValue(device.Channels[25]); set => auxLevelProp.SetValue(device.Channels[25], value); }
        public float Ch27 { get => (float)auxLevelProp.GetValue(device.Channels[26]); set => auxLevelProp.SetValue(device.Channels[26], value); }
        public float Ch28 { get => (float)auxLevelProp.GetValue(device.Channels[27]); set => auxLevelProp.SetValue(device.Channels[27], value); }
        public float Ch29 { get => (float)auxLevelProp.GetValue(device.Channels[28]); set => auxLevelProp.SetValue(device.Channels[28], value); }
        public float Ch30 { get => (float)auxLevelProp.GetValue(device.Channels[29]); set => auxLevelProp.SetValue(device.Channels[29], value); }
        public float Ch31 { get => (float)auxLevelProp.GetValue(device.Channels[30]); set => auxLevelProp.SetValue(device.Channels[30], value); }
        public float Ch32 { get => (float)auxLevelProp.GetValue(device.Channels[31]); set => auxLevelProp.SetValue(device.Channels[31], value); }
        public float Ch33 { get => (float)auxLevelProp.GetValue(device.Channels[32]); set => auxLevelProp.SetValue(device.Channels[32], value); }
        public float Ch34 { get => (float)auxLevelProp.GetValue(device.Channels[33]); set => auxLevelProp.SetValue(device.Channels[33], value); }
        public float Ch35 { get => (float)auxLevelProp.GetValue(device.Channels[34]); set => auxLevelProp.SetValue(device.Channels[34], value); }
        public float Ch36 { get => (float)auxLevelProp.GetValue(device.Channels[35]); set => auxLevelProp.SetValue(device.Channels[35], value); }
        public float Ch37 { get => (float)auxLevelProp.GetValue(device.Channels[36]); set => auxLevelProp.SetValue(device.Channels[36], value); }
        public float Ch38 { get => (float)auxLevelProp.GetValue(device.Channels[37]); set => auxLevelProp.SetValue(device.Channels[37], value); }
        public float Ch39 { get => (float)auxLevelProp.GetValue(device.Channels[38]); set => auxLevelProp.SetValue(device.Channels[38], value); }



        public float Ch1Pan { get => (float)auxPanProp.GetValue(device.Channels[0]); set => auxPanProp.SetValue(device.Channels[0], value); }
        public float Ch2Pan { get => (float)auxPanProp.GetValue(device.Channels[1]); set => auxPanProp.SetValue(device.Channels[1], value); }
        public float Ch3Pan { get => (float)auxPanProp.GetValue(device.Channels[2]); set => auxPanProp.SetValue(device.Channels[2], value); }
        public float Ch4Pan { get => (float)auxPanProp.GetValue(device.Channels[3]); set => auxPanProp.SetValue(device.Channels[3], value); }
        public float Ch5Pan { get => (float)auxPanProp.GetValue(device.Channels[4]); set => auxPanProp.SetValue(device.Channels[4], value); }
        public float Ch6Pan { get => (float)auxPanProp.GetValue(device.Channels[5]); set => auxPanProp.SetValue(device.Channels[5], value); }
        public float Ch7Pan { get => (float)auxPanProp.GetValue(device.Channels[6]); set => auxPanProp.SetValue(device.Channels[6], value); }
        public float Ch8Pan { get => (float)auxPanProp.GetValue(device.Channels[7]); set => auxPanProp.SetValue(device.Channels[7], value); }
        public float Ch9Pan { get => (float)auxPanProp.GetValue(device.Channels[8]); set => auxPanProp.SetValue(device.Channels[8], value); }
        public float Ch10Pan { get => (float)auxPanProp.GetValue(device.Channels[9]); set => auxPanProp.SetValue(device.Channels[9], value); }
        public float Ch11Pan { get => (float)auxPanProp.GetValue(device.Channels[10]); set => auxPanProp.SetValue(device.Channels[10], value); }
        public float Ch12Pan { get => (float)auxPanProp.GetValue(device.Channels[11]); set => auxPanProp.SetValue(device.Channels[11], value); }
        public float Ch13Pan { get => (float)auxPanProp.GetValue(device.Channels[12]); set => auxPanProp.SetValue(device.Channels[12], value); }
        public float Ch14Pan { get => (float)auxPanProp.GetValue(device.Channels[13]); set => auxPanProp.SetValue(device.Channels[13], value); }
        public float Ch15Pan { get => (float)auxPanProp.GetValue(device.Channels[14]); set => auxPanProp.SetValue(device.Channels[14], value); }
        public float Ch16Pan { get => (float)auxPanProp.GetValue(device.Channels[15]); set => auxPanProp.SetValue(device.Channels[15], value); }
        public float Ch17Pan { get => (float)auxPanProp.GetValue(device.Channels[16]); set => auxPanProp.SetValue(device.Channels[16], value); }
        public float Ch18Pan { get => (float)auxPanProp.GetValue(device.Channels[17]); set => auxPanProp.SetValue(device.Channels[17], value); }
        public float Ch19Pan { get => (float)auxPanProp.GetValue(device.Channels[18]); set => auxPanProp.SetValue(device.Channels[18], value); }
        public float Ch20Pan { get => (float)auxPanProp.GetValue(device.Channels[19]); set => auxPanProp.SetValue(device.Channels[19], value); }
        public float Ch21Pan { get => (float)auxPanProp.GetValue(device.Channels[20]); set => auxPanProp.SetValue(device.Channels[20], value); }
        public float Ch22Pan { get => (float)auxPanProp.GetValue(device.Channels[21]); set => auxPanProp.SetValue(device.Channels[21], value); }
        public float Ch23Pan { get => (float)auxPanProp.GetValue(device.Channels[22]); set => auxPanProp.SetValue(device.Channels[22], value); }
        public float Ch24Pan { get => (float)auxPanProp.GetValue(device.Channels[23]); set => auxPanProp.SetValue(device.Channels[23], value); }
        public float Ch25Pan { get => (float)auxPanProp.GetValue(device.Channels[24]); set => auxPanProp.SetValue(device.Channels[24], value); }
        public float Ch26Pan { get => (float)auxPanProp.GetValue(device.Channels[25]); set => auxPanProp.SetValue(device.Channels[25], value); }
        public float Ch27Pan { get => (float)auxPanProp.GetValue(device.Channels[26]); set => auxPanProp.SetValue(device.Channels[26], value); }
        public float Ch28Pan { get => (float)auxPanProp.GetValue(device.Channels[27]); set => auxPanProp.SetValue(device.Channels[27], value); }
        public float Ch29Pan { get => (float)auxPanProp.GetValue(device.Channels[28]); set => auxPanProp.SetValue(device.Channels[28], value); }
        public float Ch30Pan { get => (float)auxPanProp.GetValue(device.Channels[29]); set => auxPanProp.SetValue(device.Channels[29], value); }
        public float Ch31Pan { get => (float)auxPanProp.GetValue(device.Channels[30]); set => auxPanProp.SetValue(device.Channels[30], value); }
        public float Ch32Pan { get => (float)auxPanProp.GetValue(device.Channels[31]); set => auxPanProp.SetValue(device.Channels[31], value); }
        public float Ch33Pan { get => (float)auxPanProp.GetValue(device.Channels[32]); set => auxPanProp.SetValue(device.Channels[32], value); }
        public float Ch34Pan { get => (float)auxPanProp.GetValue(device.Channels[33]); set => auxPanProp.SetValue(device.Channels[33], value); }
        public float Ch35Pan { get => (float)auxPanProp.GetValue(device.Channels[34]); set => auxPanProp.SetValue(device.Channels[34], value); }
        public float Ch36Pan { get => (float)auxPanProp.GetValue(device.Channels[35]); set => auxPanProp.SetValue(device.Channels[35], value); }
        public float Ch37Pan { get => (float)auxPanProp.GetValue(device.Channels[36]); set => auxPanProp.SetValue(device.Channels[36], value); }
        public float Ch38Pan { get => (float)auxPanProp.GetValue(device.Channels[37]); set => auxPanProp.SetValue(device.Channels[37], value); }
        public float Ch39Pan { get => (float)auxPanProp.GetValue(device.Channels[38]); set => auxPanProp.SetValue(device.Channels[38], value); }


    }
}

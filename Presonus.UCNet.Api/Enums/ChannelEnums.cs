using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Enums
{
    public enum BusChannelRoutingMode { PreFader = 0, PostFX, PostFader }
    public enum DawPostDsp { Pre = 0, Post }
    public enum LineInputSource { Analog = 0, Network, USB, SD }
    public enum ReturnInputSource { Network = 0, USB = 1, SD = 2 }
    public enum BusSource { Mixer = 0, Network }

    public enum HeadphoneSource
    {
        Main = 0, Solo, Tape, Digital,
        Mix1, Mix2, Mix3, Mix4, Mix5, Mix6, Mix7, Mix8, Mix9, Mix10, Mix11, Mix12, Mix13, Mix14, Mix15, Mix16
    }

    public enum ADCSource
    {
        Input1 = 1, Input2, Input3, Input4, Input5, Input6, Input7, Input8,
        Input9, Input10, Input11, Input12, Input13, Input14, Input15, Input16,
        Input17, Input18, Input19, Input20, Input21, Input22, Input23, Input24,
        Input25, Input26, Input27, Input28, Input29, Input30, Input31, Input32
    }

    public enum AVBSource
    {
        Input1 = 1, Input2, Input3, Input4, Input5, Input6, Input7, Input8,
        Input9, Input10, Input11, Input12, Input13, Input14, Input15, Input16,
        Input17, Input18, Input19, Input20, Input21, Input22, Input23, Input24,
        Input25, Input26, Input27, Input28, Input29, Input30, Input31, Input32,
        Input33, Input34, Input35, Input36, Input37, Input38, Input39, Input40,
        Input41, Input42, Input43, Input44, Input45, Input46, Input47, Input48,
        Input49, Input50, Input51, Input52, Input53, Input54, Input55, Input56,
        Input57, Input58, Input59, Input60, Input61, Input62, Input63, Input64
    }

    public enum USBSource
    {
        Input1 = 1, Input2, Input3, Input4, Input5, Input6, Input7, Input8,
        Input9, Input10, Input11, Input12, Input13, Input14, Input15, Input16,
        Input17, Input18, Input19, Input20, Input21, Input22, Input23, Input24,
        Input25, Input26, Input27, Input28, Input29, Input30, Input31, Input32,
        Input33, Input34, Input35, Input36, Input37, Input38, Input39, Input40,
        Input41, Input42, Input43, Input44, Input45, Input46, Input47, Input48,
        Input49, Input50, Input51, Input52, Input53, Input54, Input55, Input56,
        Input57, Input58, Input59, Input60, Input61, Input62, Input63, Input64
    }
}

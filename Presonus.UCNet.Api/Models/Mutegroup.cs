using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
    public class Mutegroup : ParameterRouter
    {
        public Mutegroup(MixerStateService mixerStateService) : base("mutegroup", -1, mixerStateService)
        {
        }

        public bool allon { get => GetBoolean(); set => SetBoolean(value); }
        public bool alloff { get => GetBoolean(); set => SetBoolean(value); }

        public bool mutegroup1 { get => GetBoolean(); set => SetBoolean(value); }
        public bool mutegroup2 { get => GetBoolean(); set => SetBoolean(value); }
        public bool mutegroup3 { get => GetBoolean(); set => SetBoolean(value); }
        public bool mutegroup4 { get => GetBoolean(); set => SetBoolean(value); }
        public bool mutegroup5 { get => GetBoolean(); set => SetBoolean(value); }
        public bool mutegroup6 { get => GetBoolean(); set => SetBoolean(value); }
        public bool mutegroup7 { get => GetBoolean(); set => SetBoolean(value); }
        public bool mutegroup8 { get => GetBoolean(); set => SetBoolean(value); }

        public string mutegroup1username { get => GetString(); set => SetString(value); }
        public string mutegroup2username { get => GetString(); set => SetString(value); }
        public string mutegroup3username { get => GetString(); set => SetString(value); }
        public string mutegroup4username { get => GetString(); set => SetString(value); }
        public string mutegroup5username { get => GetString(); set => SetString(value); }
        public string mutegroup6username { get => GetString(); set => SetString(value); }
        public string mutegroup7username { get => GetString(); set => SetString(value); }
        public string mutegroup8username { get => GetString(); set => SetString(value); }

        public string mutegroup1mutes { get => GetString(); set => SetString(value); }
        public string mutegroup2mutes { get => GetString(); set => SetString(value); }
        public string mutegroup3mutes { get => GetString(); set => SetString(value); }
        public string mutegroup4mutes { get => GetString(); set => SetString(value); }
        public string mutegroup5mutes { get => GetString(); set => SetString(value); }
        public string mutegroup6mutes { get => GetString(); set => SetString(value); }
        public string mutegroup7mutes { get => GetString(); set => SetString(value); }
        public string mutegroup8mutes { get => GetString(); set => SetString(value); }

        public void AssignMutesToAGroup(int index)
        {
            _mixerStateService.AssignMutes(index);
        }

        public static List<ChannelTypes> ChannelOrder = new()
        {
            ChannelTypes.LINE,
            ChannelTypes.RETURN,
            ChannelTypes.FXRETURN,
            ChannelTypes.AUX,
            ChannelTypes.FX,
            ChannelTypes.SUB,
            ChannelTypes.MAIN
        };

        public bool GetChannelMuteStatus(int mutegroup, ChannelSelector channelSelector)
        {
            if (mutegroup < 1 || mutegroup > 8)
                throw new ArgumentOutOfRangeException(nameof(mutegroup));

            if (channelSelector == null)
                throw new ArgumentNullException(nameof(channelSelector));

            if (channelSelector.Channel < 0 || channelSelector.Channel > Mixer.ChannelCounts[channelSelector.Type])
                throw new ArgumentOutOfRangeException(nameof(channelSelector.Channel));

            string mutes = mutegroup switch
            {
                1 => mutegroup1mutes,
                2 => mutegroup2mutes,
                3 => mutegroup3mutes,
                4 => mutegroup4mutes,
                5 => mutegroup5mutes,
                6 => mutegroup6mutes,
                7 => mutegroup7mutes,
                8 => mutegroup8mutes,
                _ => string.Empty // Default case if none of the above match
            };

            int indexOfThisChannel = -1;

            foreach (var channelType in ChannelOrder)
            {
                if (channelType == channelSelector.Type)
                {
                    indexOfThisChannel += channelSelector.Channel;
                    break;
                }
                else
                {
                    indexOfThisChannel += Mixer.ChannelCounts[channelType];
                }
            }

            if (indexOfThisChannel < 0 || indexOfThisChannel >= mutes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(channelSelector));
                // or return a default value, e.g., false
                // return false;
            }

            char channelMuteStatus = mutes[indexOfThisChannel];
            return channelMuteStatus == '1';
        }


        private string ListToString(List<bool> mutes)
        {
            StringBuilder stringBuilder = new StringBuilder(mutes.Count);
            foreach (var state in mutes)
            {
                stringBuilder.Append(state ? "1" : 0);
            }
            return stringBuilder.ToString();
        }

        public void SetChannelMutes(int mutegroup, List<bool> mutes)
        {
            string mutesString = ListToString(mutes);

            switch (mutegroup)
            {
                case 1:
                    mutegroup1mutes = mutesString;
                    break;
                case 2:
                    mutegroup2mutes = mutesString;
                    break;
                case 3:
                    mutegroup3mutes = mutesString;
                    break;
                case 4:
                    mutegroup4mutes = mutesString;
                    break;
                case 5:
                    mutegroup5mutes = mutesString;
                    break;
                case 6:
                    mutegroup6mutes = mutesString;
                    break;
                case 7:
                    mutegroup7mutes = mutesString;
                    break;
                case 8:
                    mutegroup8mutes = mutesString;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mutegroup));
            }
        }


        public void SetChannelMuteStatus(int mutegroup, ChannelSelector channelSelector, bool assign)
        {
            if (mutegroup < 1 || mutegroup > 8)
                throw new ArgumentOutOfRangeException(nameof(mutegroup));

            if (channelSelector == null)
                throw new ArgumentNullException(nameof(channelSelector));

            if (channelSelector.Channel < 0 || channelSelector.Channel > Mixer.ChannelCounts[channelSelector.Type])
                throw new ArgumentOutOfRangeException(nameof(channelSelector.Channel));

            string mutes = mutegroup switch
            {
                1 => mutegroup1mutes,
                2 => mutegroup2mutes,
                3 => mutegroup3mutes,
                4 => mutegroup4mutes,
                5 => mutegroup5mutes,
                6 => mutegroup6mutes,
                7 => mutegroup7mutes,
                8 => mutegroup8mutes,
                _ => string.Empty // Default case if none of the above match
            };

            int indexOfThisChannel = -1;

            foreach (var channelType in ChannelOrder)
            {
                if (channelType == channelSelector.Type)
                {
                    indexOfThisChannel += channelSelector.Channel;
                    break;
                }
                else
                {
                    indexOfThisChannel += Mixer.ChannelCounts[channelType];
                }
            }

            if (indexOfThisChannel < 0 || indexOfThisChannel >= mutes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(channelSelector));
                // or return a default value, e.g., false
                // return false;
            }

            char newChar = assign ? '1' : '0';
            var mutesString = mutes.Substring(0, indexOfThisChannel) + newChar + mutes.Substring(indexOfThisChannel + 1);
            switch (mutegroup)
            {
                case 1:
                    mutegroup1mutes = mutesString;
                    break;
                case 2:
                    mutegroup2mutes = mutesString;
                    break;
                case 3:
                    mutegroup3mutes = mutesString;
                    break;
                case 4:
                    mutegroup4mutes = mutesString;
                    break;
                case 5:
                    mutegroup5mutes = mutesString;
                    break;
                case 6:
                    mutegroup6mutes = mutesString;
                    break;
                case 7:
                    mutegroup7mutes = mutesString;
                    break;
                case 8:
                    mutegroup8mutes = mutesString;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mutegroup));
            }
        }


        public override event PropertyChangedEventHandler PropertyChanged;

        public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}

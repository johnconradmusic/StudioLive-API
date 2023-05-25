using System;
using System.Collections.Generic;

namespace Presonus.UCNet.Api.Models
{
	public enum ChannelTypes
	{
		LINE,
		RETURN,
		TALKBACK,
		FX,
		AUX,
		FXRETURN,
		SUB,
		MAIN,
		NONE,
		GEQ
	}

	public class ChannelUtil
	{
		private static readonly Dictionary<ChannelTypes, string> ChannelStrings = new Dictionary<ChannelTypes, string>()
	{
		{ ChannelTypes.LINE, "line" },
		{ ChannelTypes.MAIN, "main" },
		{ ChannelTypes.TALKBACK, "talkback" },
		{ ChannelTypes.AUX, "aux" },
		{ ChannelTypes.SUB, "sub" },
		{ ChannelTypes.FX, "fx" },
		{ ChannelTypes.FXRETURN, "fxreturn" },
		{ ChannelTypes.RETURN, "return" }
	};

		public static string GetChannelString(ChannelSelector selector)
		{
			var channelType = selector.Type;
			var channelIndex = selector.Channel;
			var mixSourceType = selector.MixType;
			var mixSourceIndex = selector.MixNumber;

			if (!Enum.IsDefined(typeof(ChannelTypes), channelType))
			{
				throw new ArgumentException($"Invalid channel type '{channelType}' provided");
			}

			if (channelType == ChannelTypes.MAIN || channelType == ChannelTypes.TALKBACK)
			{
				channelIndex = 1;
			}

			if (channelIndex < 1)
			{
				throw new ArgumentException($"Invalid channel index '{channelIndex}' provided");
			}

			if (mixSourceType.HasValue && mixSourceIndex.HasValue)
			{
				if (!Enum.IsDefined(typeof(ChannelTypes), mixSourceType.Value))
				{
					throw new ArgumentException($"Invalid mix source type '{mixSourceType}' provided");
				}

				if (mixSourceType.Value == ChannelTypes.AUX || mixSourceType.Value == ChannelTypes.FX)
				{
					if (Mixer.ChannelCounts != null && !(mixSourceIndex <= Mixer.ChannelCounts[mixSourceType.Value]))
					{
						throw new ArgumentException($"Invalid mix source index '{mixSourceIndex}' provided");
					}
				}
				else
				{
					throw new ArgumentException($"Mix source type '{mixSourceType}' is not allowed");
				}
			}

			var channelString = $"{ChannelStrings[channelType]}/ch{channelIndex}";
			return channelString;
		}
	}

	public class ChannelSelector
	{
		public ChannelSelector(ChannelTypes type, int channel, ChannelTypes? mixType = null, int? mixNumber = null)
		{
			Type = type;
			Channel = channel;
			MixType = mixType;
			MixNumber = mixNumber;
		}
		public ChannelSelector(Channel channel)
		{
			Type = channel.ChannelType;
			Channel = channel.ChannelIndex;
			MixType = null;
			MixNumber = null;
		}
		public string GetChannelString => ChannelUtil.GetChannelString(this);
		public ChannelTypes Type { get; }
		public int Channel { get; }
		public ChannelTypes? MixType { get; }
		public int? MixNumber { get; }
	}
}
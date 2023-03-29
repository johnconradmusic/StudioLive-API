using System;
using System.Collections.Generic;
using System.Linq;

namespace Presonus.UCNet.Api.NewDataModel
{

	public enum ChannelTypes
	{
		LINE,
		MAIN,
		TALKBACK,
		AUX,
		SUB,
		FX,
		FXRETURN,
		RETURN
	}

	public class ChannelStrings
	{
		public const string LINE = "line";
		public const string MAIN = "main";
		public const string TALKBACK = "talkback";
		public const string AUX = "aux";
		public const string SUB = "sub";
		public const string FX = "fxbus";
		public const string FXRETURN = "fxreturn";
		public const string RETURN = "return";
	}

	public class ChannelCount : Dictionary<ChannelTypes, int>
	{
	}

	public class ChannelSelector
	{
		public ChannelTypes type;
		public int? channel;
		public ChannelTypes? mixType;
		public int? mixNumber;

		public ChannelSelector(ChannelTypes type, int? channel, ChannelTypes? mixType, int? mixNumber)
		{
			this.type = type;
			this.channel = channel;
			this.mixType = mixType;
			this.mixNumber = mixNumber;
		}
	}

	public static class ChannelUtil
	{
		private static ChannelCount counts;

		public static void SetCounts(ChannelCount channelCount)
		{
			counts = channelCount;
		}

		// FIXME: Add channel whitelist
		public static string GetChannelString(ChannelSelector selector, List<ChannelTypes> whitelist = null)
		{
			var type = selector.type;
			double channel = selector.channel ?? 0;
			var mixType = selector.mixType;
			var mixNumber = selector.mixNumber;

			if (counts != null && mixType != null)
			{
				if (!mixNumber.HasValue || mixNumber.Value < 1)
				{
					throw new Exception("Invalid mix channel provided");
				}

				switch (mixType.Value)
				{
					case ChannelTypes.AUX:
					case ChannelTypes.FX:
						if (!(mixNumber <= counts[mixType.Value])) throw new Exception("Invalid mix channel provided");
						break;
					default:
						throw new Exception("Invalid mix type provided");
				}
			}

			// `type` must be a valid enum key
			if (!Enum.IsDefined(typeof(ChannelTypes), type))
			{
				throw new Exception("Invalid channel type provided");
			}

			if (new[] { ChannelTypes.MAIN, ChannelTypes.TALKBACK }.Contains(type))
			{
				// Force channel = 1 for main and talkback channels
				channel = 1;
			}

			if (
				// `channel` must be a whole number larger than zero
				!(Math.Truncate(channel) > 0) ||
				(channel != (channel = Math.Truncate(channel))) || // eslint-disable-line eqeqeq

				// `channel` must also be less than the known maximum (if a max is provided)
				(counts != null && !(counts.ContainsKey(type) && counts[type] >= channel))
			)
			{
				throw new Exception("Invalid channel provided");
			}

			return $"{ChannelUtil.Channel[type]}/ch{channel}";
		}

		public static readonly Dictionary<ChannelTypes, string> Channel = new Dictionary<ChannelTypes, string>()
		{
			{ ChannelTypes.LINE, ChannelStrings.LINE },
			{ ChannelTypes.MAIN, ChannelStrings.MAIN },
			{ ChannelTypes.TALKBACK, ChannelStrings.TALKBACK },
			{ ChannelTypes.AUX, ChannelStrings.AUX },
			{ ChannelTypes.SUB, ChannelStrings.SUB },
			{ ChannelTypes.FX, ChannelStrings.FX },
			{ ChannelTypes.FXRETURN, ChannelStrings.FXRETURN },
			{ ChannelTypes.RETURN, ChannelStrings.RETURN }
		};
	}
}

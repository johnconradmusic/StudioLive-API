using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Messages
{
	public class MessageCode
	{
		public const string KeepAlive = "KA";
		public const string Hello = "UM";
		public const string JSON = "JM";
		public const string CompressedJSON = "ZM";

		public const string ParamValue = "PV";

		public const string ParamColor = "PC";
		public const string ParamString = "PS";

		public const string ParamStrList = "PL";

		public const string FileRequest = "FR";
		public const string FileData = "FD";
		public const string ZLIB = "ZB";
		public const string Unknown1 = "BO";
		public const string Chunk = "CK";
		public const string Unknown3 = "MB";

		public const string FaderPosition = "MS";
		public const string MeterSingles = "MS";

		public const string Meter = "levl";
		public const string Reduction = "redu";
		public const string RTA = "rtan";
	}
}

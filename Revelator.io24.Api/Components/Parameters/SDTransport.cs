using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class SDTransport : ParameterBase
	{
		public Param play;
		public Param stop;
		public Param record;
		public Param returnToZero;
		public Param fastForward;
		public Param rewind;
		public Param recordLock = new();
		public Param location;
		public Param locate_pos;
		public Param currentRecordTime;
		public Param jog;
		public Param running;
		public Param remainingTime;
		public Param diskSpaceWarning = new();
		public Param fileSizeWarning = new();
		public Param performanceWarning;
		public Param record_error;
		public Param lockTransport = new();

		public SDTransport(string path) : base(path)
		{
		}
	}
}

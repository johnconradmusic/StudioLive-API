using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Models
{
	public class MeterData
	{
		public Dictionary<string, CircularBuffer<ushort>> Data { get; set; }
	}
}

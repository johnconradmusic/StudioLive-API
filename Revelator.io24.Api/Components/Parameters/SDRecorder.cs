using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class SDRecorder : ParameterBase
	{
		public Param soundcheckMode = new();
		public Param allDigitalInput = new();
		public Param armAll = new();
		public Param armSelect = new();
		public Param status;

		public SDRecorder(string path) : base(path)
		{
		}
	}
}

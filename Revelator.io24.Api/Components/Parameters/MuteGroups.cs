using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class MuteGroups : ParameterBase
	{
		public Param allon;
		public Param alloff;
		public Param mutegroup1;
		public Param mutegroup2;
		public Param mutegroup3;
		public Param mutegroup4;
		public Param mutegroup5;
		public Param mutegroup6;
		public Param mutegroup7;
		public Param mutegroup8;
		public Param mutegroup1username = new("Mute Group 1 Name", ParamType.STRING);
		public Param mutegroup2username = new("Mute Group 2 Name", ParamType.STRING);
		public Param mutegroup3username = new("Mute Group 3 Name", ParamType.STRING);
		public Param mutegroup4username = new("Mute Group 4 Name", ParamType.STRING);
		public Param mutegroup5username = new("Mute Group 5 Name", ParamType.STRING);
		public Param mutegroup6username = new("Mute Group 6 Name", ParamType.STRING);
		public Param mutegroup7username = new("Mute Group 7 Name", ParamType.STRING);
		public Param mutegroup8username = new("Mute Group 8 Name", ParamType.STRING);
		public Param mutegroup1mutes = new("Mute Group 1 Mutes", ParamType.STRING);
		public Param mutegroup2mutes = new("Mute Group 2 Mutes", ParamType.STRING);
		public Param mutegroup3mutes = new("Mute Group 3 Mutes", ParamType.STRING);
		public Param mutegroup4mutes = new("Mute Group 4 Mutes", ParamType.STRING);
		public Param mutegroup5mutes = new("Mute Group 5 Mutes", ParamType.STRING);
		public Param mutegroup6mutes = new("Mute Group 6 Mutes", ParamType.STRING);
		public Param mutegroup7mutes = new("Mute Group 7 Mutes", ParamType.STRING);
		public Param mutegroup8mutes = new("Mute Group 8 Mutes", ParamType.STRING);
		public Param safe_mutes = new("Safe Mutes", ParamType.STRING);

		public MuteGroups(string path) : base(path)
		{
		}
	}
}

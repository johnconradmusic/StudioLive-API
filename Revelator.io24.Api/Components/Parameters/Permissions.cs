using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class Permissions : ParameterBase
	{
		public Param device_list;
		public Param mix_permissions;
		public Param mix_save = new("Mix Save Value", ParamType.STRING);
		public Param access_code;
		public Param wheel_only;
		public Param rename;
		public Param channel_source;
		public Param preamps;
		public Param assigns;
		public Param scenes;
		public Param mute_groups;
		public Param channel_type;
		public Param eq_dyn;
		public Param geq;
		public Param fx;
		public Param groups;

		public Permissions(string path) : base(path)
		{
		}
	}
}

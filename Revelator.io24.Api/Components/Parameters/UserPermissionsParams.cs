using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class UserPermissionsParams : ParameterBase
	{
		public Param settings;
		public Param routing;
		public Param soft_patching;
		public Param ucnet;
		public Param scene_locking;
		public Param channel_names;
		public Param channel_type;
		public Param input_dsp;
		public Param output_dsp;
		public Param preamps;
		public Param geq;
		public Param assigns;
		public Param fx_types;
		public Param save_scenes;
		public Param edit_dca;
		public Param edit_mute_groups;
		public Param daw_control;

		public UserPermissionsParams(string path) : base(path)
		{
		}
	}
}

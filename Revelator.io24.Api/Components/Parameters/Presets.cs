using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class Presets : ParameterBase
	{
		public Param loaded_scene_name = new("Loaded Scene", ParamType.STRING);
		public Param loaded_scene_title = new("Loaded Scene", ParamType.STRING);
		public Param loading_scene;
		public Param loaded_project_name = new("Loaded Project", ParamType.STRING);
		public Param loaded_project_title = new("Loaded Project", ParamType.STRING);
		public Param diskusage;
		public Param storedisabled = new("Disk Storage Disabled", ParamType.TOGGLE);

		public Presets(string path) : base(path)
		{
		}
	}
}

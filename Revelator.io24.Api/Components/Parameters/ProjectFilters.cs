using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class ProjectFilters : ParameterBase
	{
		public Param fltr_input_source;
		public Param fltr_flexmixmode;
		public Param fltr_flexmixprepostmode;
		public Param fltr_fxmixpreposmode;
		public Param fltr_talkbackassigns;
		public Param fltr_solosettings;
		public Param fltr_generalsettings;
		public Param fltr_avbstreamrouting;
		public Param fltr_inputpatching;
		public Param fltr_outputpatching;
		public Param fltr_avbpatching;
		public Param fltr_sdpatching;
		public Param fltr_usbpatching;
		public Param fltr_geq;
		public Param fltr_user_functions;

		public ProjectFilters(string path) : base(path)
		{
		}
	}
}

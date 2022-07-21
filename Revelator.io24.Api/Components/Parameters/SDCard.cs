using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class SDCard : ParameterBase
	{
		public Param card_type;
		public Param card_mounted = new();
		public Param card_capcity;
		public Param volume_label;
		public Param speed_test = new();
		public Param performance_speed;
		public Param performance_tracks;
		public Param format_type;
		public Param do_format = new();
		public Param format_error = new();
		public Param progress;
		public Param scancard = new();

		public SDCard(string path) : base(path)
		{
		}
	}
}

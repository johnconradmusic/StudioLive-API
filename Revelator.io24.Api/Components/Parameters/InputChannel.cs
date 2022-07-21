using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class InputChannel : RoutableChannel
	{
		public Param lr;
		public Param sub_asn_flags;
		public Param sub1;
		public Param sub2;
		public Param sub3;
		public Param sub4;
		public Param fx_asn_flags;
		public Param assign_fx1;
		public Param assign_fx2;
		public Param assign_fx3;
		public Param assign_fx4;
		public Param assign_fx5;
		public Param assign_fx6;
		public Param assign_fx7;
		public Param assign_fx8;
		public Param FXA;
		public Param FXB;
		public Param FXC;
		public Param FXD;
		public Param FXE;
		public Param FXF;
		public Param FXG;
		public Param FXH;
		public Param inputsrc;
		public Param inputsrc_preview;
		public Param delay;
		public Param flexassignflags;

		public InputChannel(string path) : base(path)
		{
		}
	}
}

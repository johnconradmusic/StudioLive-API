using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	internal class SDSession : ParameterBase
	{
		public Param artist;
		public Param performance;
		public Param location;
		public Param artistlist;
		public Param performancelist;
		public Param locationlist;
		public Param folder_hierarchy;
		public Param title;
		public Param path;
		public Param _new;
		public Param load;
		public Param loadmix;
		public Param close;
		public Param active;
		public Param lockSession = new();
		public Param loadStatus;

		public SDSession(string path) : base(path)
		{
		}
	}
}

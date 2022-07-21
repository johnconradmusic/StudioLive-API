using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class UserProfileParams : ParameterBase
	{
		public Param username;
		public Param avatar;
		public Param path;
		public Param project;
		public Param scene;
		public Param password_hash;
		public Param is_admin;
		public Param avatar_index;
		public Param level_limit_value;

		public UserProfileParams(string path) : base(path)
		{
		}
	}
}

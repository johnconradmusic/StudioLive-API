using Presonus.UC.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.StudioLive32.Api
{
	public abstract class Parameter : IRoutable
	{
		public string Name { get; set; }

		public string _routePrefix;
		public string Route
		{
			get => _routePrefix + "/" + Name.ToLower();
		}

		public Parameter(string _routePrefix)
		{
			this._routePrefix = _routePrefix;
		}

		//"line/ch3/volume"
	}
	public class FloatParameter : Parameter
	{
		public float Value { get=>GetValue }

	}


}

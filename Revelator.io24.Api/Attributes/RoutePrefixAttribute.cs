using Presonus.StudioLive32.Api.Enums;
using System;

namespace Presonus.StudioLive32.Api.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class ParameterPathAttribute : Attribute
	{
		public ParameterPathAttribute(string routeValueName)
		{
			ParameterPath = routeValueName;
		}
		public string ParameterPath { get; }
	}


}

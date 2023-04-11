using System;

namespace Presonus.UCNet.Api.Attributes
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

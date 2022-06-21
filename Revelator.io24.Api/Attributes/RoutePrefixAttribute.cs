using Revelator.io24.Api.Enums;
using System;

namespace Revelator.io24.Api.Attributes
{
	//[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	//public class RoutePrefixAttribute : Attribute
	//{
	//    public RoutePrefixAttribute(string routePrefix)
	//    {
	//        RoutePrefixName = routePrefix;
	//    }

	//    public string RoutePrefixName { get; }
	//}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class RouteValueAttribute : Attribute
	{
		public RouteValueAttribute(string routeValueName)
		{
			RouteValueName = routeValueName;
		}
		public string RouteValueName { get; }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class RouteValueRangeAttribute : Attribute
	{
		public RouteValueRangeAttribute(float min = 0, float max = 1, Unit unit = Unit.none)
		{
			Min = min;
			Max = max;
			Unit = unit;
		}
		public Unit Unit { get; }
		public float Min { get; }
		public float Max { get; }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class RouteStringAttribute : Attribute
	{
		public RouteStringAttribute(string routeStringName)
		{
			RouteStringName = routeStringName;
		}

		public string RouteStringName { get; }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class RouteStringsAttribute : Attribute
	{
		public RouteStringsAttribute(string routeStringsName)
		{
			RouteStringsName = routeStringsName;
		}

		public string RouteStringsName { get; }
	}
}

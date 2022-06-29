using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.StudioLive32.Api
{
	public class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public string RoutePrefix { get; set; }
		protected RawService _rawService;
		protected virtual void SetProperty<T>(ref T member, T val,
		 [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(member, val)) return;

			var type = this.GetType();
			string route = "";
			//RoutePrefix is the channel route.. the property VALUE should have the rest of the route
			if(member is FloatParameter floatParameter)
			route = RoutePrefix + floatParameter.Route

			var routeValue = property.GetCustomAttribute<RouteValueAttribute>();
			if (routeValue != null || property.PropertyType == typeof(bool) || property.PropertyType == typeof(int) || property.PropertyType == typeof(float))
			{
				var route = routeValue != null ? $"{_routePrefix}/{routeValue.RouteValueName}" : $"{_routePrefix}/{property.Name}";
				_propertyValueNameRoute[property.Name.ToLower()] = route;
				var range = property.GetCustomAttribute<RouteValueRangeAttribute>();
				if (range != null)
				{
					_rawService._propertyValueRanges[route] = new ValueRange(range.Min, range.Max, range.Unit);
				}
				continue;
			}

			member = val;
			this.OnPropertyChanged(propertyName);

			//Property notification propagation test
			if (member.GetType().IsSubclassOf(typeof(BindableBase)))
				(member as BindableBase).PropertyChanged += BindableBase_PropertyChanged;

			_rawService.SetValue()

		}
		public BindableBase(string Route, RawService rawService)
		{
			_rawService = rawService;
			RoutePrefix = Route;
		}
		private void BindableBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(nameof(sender) + "." + e.PropertyName);
		}

		public virtual void OnPropertyChanged(string propertyName)
		{
			//Console.WriteLine("Property change: " + propertyName);
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			//PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}

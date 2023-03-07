using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presonus.StudioLive32.Api.Enums;
namespace Presonus.UC.Api.Models
{
	public class GEQ : DeviceRoutingBase, INotifyPropertyChanged
	{
		public GEQ(string routePrefix, RawService rawService) : base(routePrefix, rawService)
		{

		}

		[RouteValueRange(-15, 15, Unit.db)] public float geq1 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq10 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq11 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq12 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq13 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq14 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq15 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq16 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq17 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq18 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq19 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq20 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq21 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq22 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq23 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq24 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq25 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq26 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq27 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq28 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq29 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq30 { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-15, 15, Unit.db)] public float geq31 { get => GetValue(); set => SetValue(value); }


		public override event PropertyChangedEventHandler PropertyChanged;

		protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			//Console.WriteLine(eventArgs.PropertyName + " CHANGED?");
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presonus.UC.Api.Components;
using Presonus.UC.Api.Helpers;

namespace Presonus.StudioLive32.Api.Console;


public class ConsoleFloatDial : ConsoleDial, INotifyPropertyChanged
{
	public override string ToString()
	{
		return Name + " " + Address;
	}

	public ConsoleFloatDial()
	{
		//RawService.instance.ValueStateUpdated += Instance_ValueStateUpdated;
	}

	private void Instance_ValueStateUpdated(string route, float value)
	{
		if (route == Address) OnPropertyChanged(new(nameof(Value)));
	}

	public float Value
	{
		get => (float)GetValue();
		set => SetValue(value);
	}

	public float Def { get; set; } = 0;
	public float Min { get; set; } = 0;
	public float Max { get; set; } = 1;
	public float Mid { get; set; } = 0.5f;
	public ParamCurve Curve { get; set; }
	protected void SetValue(float value)
	{
		//TODO: get curves worked out
		switch (Curve)
		{
			case ParamCurve.FADER:
			case ParamCurve.LOG:
			case ParamCurve.EXP:
				value = Util.ExpPercentageFromGain((float)value);
				break;
			case ParamCurve.LINEAR:
				break;
		}
		RawService.instance.SetValue(Address, (float)value);
	}

	protected float GetValue()
	{
		var value = RawService.instance.GetValue(Address);
		switch (Curve)
		{
			case ParamCurve.FADER:
			case ParamCurve.LOG:
				break;
			case ParamCurve.EXP:
				value = Util.ExpGainFromPercentage(value);
				break;
			case ParamCurve.LINEAR:
				break;
		}

		return value;
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
	{
		PropertyChanged?.Invoke(this, eventArgs);
		System.Console.WriteLine("Property changed on {0}", Name);
	}
}

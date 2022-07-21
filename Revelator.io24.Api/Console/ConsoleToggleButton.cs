using Presonus.StudioLive32.Api.Enums;
using Presonus.UC.Api.Components;

namespace Presonus.StudioLive32.Api.Console;

public class ConsoleToggleButton : ConsoleButton
{
	public string Value
	{
		get => (string)GetValue();
		set => SetValue(value);
	}

	protected object GetValue()
	{
		var floatVal = RawService.instance.GetValue(Address);
		int index = (int)(floatVal * Units.Count);
		return Units[index];
	}

	public override void Toggle()
	{
		if (Value == "")
		{
			SetValue(Units[0]);
			return;
		}
		int numOfChoices = Units.Count;
		int indexOfCurrentValue = Units.IndexOf(Value);
		SetValue(indexOfCurrentValue + 1 >= numOfChoices ? Units[0] : Units[indexOfCurrentValue + 1]);
	}

	protected void SetValue(object value)
	{
		if (!Units.Contains(value as string)) return;
		RawService.instance.SetValue(Address, Units.IndexOf(value as string) / (float)Units.Count);
	}
}
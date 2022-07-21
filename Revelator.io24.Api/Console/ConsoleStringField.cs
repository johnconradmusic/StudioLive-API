using System.ComponentModel;
using Presonus.UC.Api.Components;

namespace Presonus.StudioLive32.Api.Console;

public class ConsoleStringField : ConsoleControl
{
	protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
	{
		throw new System.NotImplementedException();
	}

	public string Value
	{
		get => (string)GetValue();
		set => SetValue(value);
	}

	protected void SetValue(string value)
	{
		var strValue = value as string;
		if (!Units.Contains(strValue)) return;
		RawService.instance.SetString(Address, strValue);
	}

	protected string GetValue()
	{
		var result = RawService.instance.GetString(Address);
		return result;
	}
}
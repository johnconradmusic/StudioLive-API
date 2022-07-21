using System.ComponentModel;

namespace Presonus.StudioLive32.Api.Console;

public class ConsoleIntDial : ConsoleDial
{
	protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
	{
		throw new System.NotImplementedException();
	}

	public int Value { get; set; }
}
using System.Collections.Generic;
using System.ComponentModel;

namespace Presonus.StudioLive32.Api.Console;

public class ConsoleEQ : ConsoleControlGroup
{
	public List<ConsoleEQBand> Band { get; set; }

	
	protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
	{
		throw new System.NotImplementedException();
	}
}
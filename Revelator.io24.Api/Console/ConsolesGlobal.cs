using System.ComponentModel;
using Presonus.StudioLive32.Api.Console;

namespace Presonus.UC.Api.Components.Parameters;

public class ConsolesGlobal : ConsoleControlGroup, INotifyPropertyChanged
{
	protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs) => PropertyChanged?.Invoke(this, eventArgs);
	public event PropertyChangedEventHandler PropertyChanged;
	public ConsolesGlobal()
	{
	}
}
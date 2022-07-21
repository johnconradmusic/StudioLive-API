using System;
using System.ComponentModel;
using Presonus.UC.Api.Components;

namespace Presonus.StudioLive32.Api.Console;

public abstract class ConsoleControl
{
	private string _Address;

	public ConsoleControl Parent;
	public string Name { get; set; }
	public ConsoleControl()
	{
		Parent = null;
		Address = ToString();
		Id = 0;
		Tag = null;
	}
	protected abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

	public ParamUnits Units { get; set; }



	public string Address
	{
		get
		{
			if (Parent is ConsoleControl)
				return Parent.Address + _Address;
			return _Address;
		}

		set => _Address = value;
	}

	public int Id { get; set; }
	public object Tag { get; set; }

}
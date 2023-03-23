using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;

namespace Presonus.UCNet.Api.Services;

public class MixerStateService
{
	private readonly MixerState _mixerState;
	private readonly MixerStateSynchronizer _mixerStateSynchronizer;

	internal Action<string, string> SendStringMethod;
	internal Action<string, float> SendValueMethod;

	public MixerStateService(MixerState mixerState, MixerStateSynchronizer mixerStateSynchronizer)
	{
		_mixerState = mixerState;
		_mixerStateSynchronizer = mixerStateSynchronizer;

		_mixerState.ValueChanged += (sender, args) => ValueChanged?.Invoke(sender, args);
		_mixerState.StringChanged += (sender, args) => StringChanged?.Invoke(sender, args);
		_mixerState.StringsChanged += (sender, args) => StringsChanged?.Invoke(sender, args);
	}

	public event EventHandler<ValueChangedEventArgs<float>> ValueChanged;

	public event EventHandler<ValueChangedEventArgs<string>> StringChanged;

	public event EventHandler<ValueChangedEventArgs<string[]>> StringsChanged;

	public event EventHandler Synchronized;


	public void Synchronize(string json)
	{
		_mixerStateSynchronizer.Synchronize(json, _mixerState);
		Synchronized?.Invoke(this, null);
	}

	public void SetString(string route, string value, bool broadcast = true)
	{
		_mixerState.SetString(route, value);
		if (broadcast)
			SendStringMethod(route, value);
	}

	public void SetStrings(string route, string[] value, bool broadcast = true)
	{
		_mixerState.SetStrings(route, value);
	}

	public void SetValue(string route, float value, bool broadcast = true)
	{
		_mixerState.SetValue(route, value);
		if (broadcast)
			SendValueMethod(route, value);
	}

	public float GetValue(string route)
	{
		return _mixerState.GetValue(route);
	}

	public string GetString(string route)
	{
		return _mixerState.GetString(route);
	}

	public string[] GetStrings(string route)
	{
		return _mixerState.GetStrings(route);
	}
}
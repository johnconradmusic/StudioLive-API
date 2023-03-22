using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;

namespace Presonus.UCNet.Api.Services;

public class MixerStateService
{
	private readonly MixerState _mixerState;
	private readonly MixerStateSynchronizer _mixerStateSynchronizer;

	private readonly Dictionary<string, float> _defaultValues = new();
	private readonly Dictionary<string, float> _midPoints = new();
	private readonly Dictionary<string, (float min, float max)> _valueRanges = new();

	internal Action<string, string, bool> SetStringMethod;
	internal Action<string, float, bool> SetValueMethod;

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

	public List<string> GetAllPaths => _mixerState.GetAllPaths();

	public void Synchronize(string json)
	{
		_mixerStateSynchronizer.Synchronize(json, _mixerState);
		Synchronized?.Invoke(this, null);
	}

	public void SetString(string route, string value, bool broadcast = true)
	{
		_mixerState.SetString(route, value);
		SetStringMethod(route, value, broadcast);
	}

	public void SetStrings(string route, string[] value, bool broadcast = true)
	{
		_mixerState.SetStrings(route, value);
	}

	public void SetValue(string route, float value, bool broadcast = true)
	{
		_mixerState.SetValue(route, value);
		SetValueMethod(route, value, broadcast);
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
using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;

namespace Presonus.UCNet.Api.Services;

public class MixerStateService
{
	private readonly MixerState _mixerState;
	private readonly MixerStateSynchronizer _mixerStateSynchronizer;
	public delegate void SyncEvent();

	internal Action<string, string> SendStringMethod;
	internal Action<string, float> SendValueMethod;
	internal Action<string> RecallProject;
	internal Action<string, string> RecallScene;

	public MixerStateService(MixerState mixerState, MixerStateSynchronizer mixerStateSynchronizer)
	{
		_mixerState = mixerState;
		_mixerStateSynchronizer = mixerStateSynchronizer;
	}

	public event EventHandler<ValueChangedEventArgs<float>> ValueChanged;

	public event EventHandler<ValueChangedEventArgs<string>> StringChanged;

	public event EventHandler<ValueChangedEventArgs<string[]>> StringsChanged;


	public void Synchronize(string json)
	{
		_mixerStateSynchronizer.Synchronize(json, this);
	}

	public void SetString(string route, string value, bool broadcast = true)
	{
		if (_mixerState.GetString(route) != value) // Check if the same value is already present
		{
			_mixerState.SetString(route, value);
			if (broadcast)
				SendStringMethod(route, value);
			StringChanged?.Invoke(this, new(route, value));
		}
	}

	public void SetStrings(string route, string[] value, bool broadcast = true)
	{
		_mixerState.SetStrings(route, value);

		StringsChanged?.Invoke(this, new(route, value));
	}

	public bool TryGetValue(string route, out float value) => _mixerState.TryGetValue(route, out value);

	public void SetValue(string route, float value, bool broadcast = true)
	{
		if (_mixerState.GetValue(route) != value) // Check if the same value is already present
		{
			_mixerState.SetValue(route, value);
			if (broadcast)
				SendValueMethod(route, value);
			ValueChanged?.Invoke(this, new(route, value));
		}
	}

	public void RecallProjectFile(string projFile)
	{
		RecallProject(projFile);
	}
	public void RecallSceneFile(string projFile, string sceneFile)
	{
		RecallScene(projFile, sceneFile);
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
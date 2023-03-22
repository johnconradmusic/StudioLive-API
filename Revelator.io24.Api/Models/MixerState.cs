using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;

namespace Presonus.UCNet.Api.Models;

public class MixerState
{
	private readonly Dictionary<string, float> _values = new();
	private readonly Dictionary<string, string> _strings = new();
	private readonly Dictionary<string, string[]> _stringArrays = new();

	public event EventHandler<ValueChangedEventArgs<float>> ValueChanged;
	public event EventHandler<ValueChangedEventArgs<string>> StringChanged;
	public event EventHandler<ValueChangedEventArgs<string[]>> StringsChanged;

	public List<string> GetAllPaths()
	{
		var allPaths = new List<string>();

		// Add paths from the _values dictionary
		allPaths.AddRange(_values.Keys);

		// Add paths from the _strings dictionary
		allPaths.AddRange(_strings.Keys);

		// Add paths from the _stringArrays dictionary
		allPaths.AddRange(_stringArrays.Keys);

		return allPaths;
	}


	public void SetValue(string path, float value)
	{

		_values[path] = value;
		ValueChanged?.Invoke(this, new ValueChangedEventArgs<float>(path, value));
	}

	public float GetValue(string path)
	{
		return _values.TryGetValue(path, out var value) ? value : default;
	}

	public void SetString(string path, string value)
	{
		_strings[path] = value;
		StringChanged?.Invoke(this, new ValueChangedEventArgs<string>(path, value));
	}

	public string GetString(string path)
	{
		return _strings.TryGetValue(path, out var value) ? value : default;
	}

	public void SetStrings(string path, string[] values)
	{
		_stringArrays[path] = values;
		StringsChanged?.Invoke(this, new ValueChangedEventArgs<string[]>(path, values));
	}

	public string[] GetStrings(string path)
	{
		return _stringArrays.TryGetValue(path, out var value) ? value : Array.Empty<string>();
	}
}

public class ValueChangedEventArgs<T> : EventArgs
{
	public string Path { get; }
	public T Value { get; }

	public ValueChangedEventArgs(string path, T value)
	{
		Path = path;
		Value = value;
	}
}

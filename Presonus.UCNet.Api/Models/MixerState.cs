using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Presonus.UCNet.Api.Models;

public class MixerState
{
	private readonly Dictionary<string, float> _values = new();
	private readonly Dictionary<string, string> _strings = new();
	private readonly Dictionary<string, string[]> _stringArrays = new();

	public void SetValue(string path, float value)
	{
		_values[path] = value;
	}

	public bool TryGetValue(string path, out float value) => _values.TryGetValue(path, out value);

	public float GetValue(string path)
	{
		return _values.TryGetValue(path, out var value) ? value : default;
	}

	public void SetString(string path, string value)
	{
		_strings[path] = value;
	}

	public string GetString(string path)
	{
		return _strings.TryGetValue(path, out var value) ? value : default;
	}

	public void SetStrings(string path, string[] values)
	{
		_stringArrays[path] = values;
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

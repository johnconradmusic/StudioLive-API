using MixingStation.Api.Models;
using System;

namespace MixingStation.Api.Services;

public sealed class MixerStateService
{
    private readonly MixerState _state;
    private readonly MixerStateSynchronizer _synchronizer;

    internal Action<string, object?>? SendMethod;

    public MixerTopology Topology { get; } = new();

    public MixerStateService(MixerState state, MixerStateSynchronizer synchronizer)
    {
        _state = state;
        _synchronizer = synchronizer;
    }

    public event EventHandler<ValueChangedEventArgs>? ValueChanged;
    public event EventHandler<ValueChangedEventArgs<float>>? FloatChanged;
    public event EventHandler<ValueChangedEventArgs<string>>? StringChanged;
    public event EventHandler<ValueChangedEventArgs<bool>>? BoolChanged;
    public event EventHandler<ValueChangedEventArgs<string[]>>? StringArrayChanged;
    public event EventHandler<ValueChangedEventArgs<MixingStationNode>>? NodeChanged;

    public void Synchronize(string json)
    {
        _synchronizer.Synchronize(json, this);
    }

    public object? GetValue(string path)
    {
        return _state.GetValue(path);
    }

    public T? GetValue<T>(string path)
    {
        return _state.GetValue<T>(path);
    }

    public bool TryGetValue<T>(string path, out T value)
    {
        return _state.TryGetValue(path, out value);
    }

    public float GetFloat(string path)
    {
        return _state.GetValue<float>(path);
    }

    public string? GetString(string path)
    {
        return _state.GetValue<string>(path);
    }

    public bool GetBool(string path)
    {
        return _state.GetValue<bool>(path);
    }

    public string[]? GetStrings(string path)
    {
        return _state.GetValue<string[]>(path);
    }

    public void SetValue(string path, object? value, bool broadcast = true)
    {
        if (!_state.SetValue(path, value))
            return;

        if (broadcast)
            SendMethod?.Invoke(path, value);

        ValueChanged?.Invoke(this, new ValueChangedEventArgs(path, value));

        switch (value)
        {
            case float f:
                FloatChanged?.Invoke(this, new ValueChangedEventArgs<float>(path, f));
                break;

            case double d:
                FloatChanged?.Invoke(this, new ValueChangedEventArgs<float>(path, (float)d));
                break;

            case int i:
                FloatChanged?.Invoke(this, new ValueChangedEventArgs<float>(path, i));
                break;

            case string s:
                StringChanged?.Invoke(this, new ValueChangedEventArgs<string>(path, s));
                break;

            case bool b:
                BoolChanged?.Invoke(this, new ValueChangedEventArgs<bool>(path, b));
                break;

            case string[] arr:
                StringArrayChanged?.Invoke(this, new ValueChangedEventArgs<string[]>(path, arr));
                break;
        }
    }

    public void SetFloat(string path, float value, bool broadcast = true)
    {
        SetValue(path, value, broadcast);
    }

    public void SetString(string path, string value, bool broadcast = true)
    {
        SetValue(path, value, broadcast);
    }

    public void SetBool(string path, bool value, bool broadcast = true)
    {
        SetValue(path, value, broadcast);
    }

    public void SetStrings(string path, string[] values, bool broadcast = true)
    {
        SetValue(path, values, broadcast);
    }

    public void SetNode(MixingStationNode node)
    {
        _state.SetNode(node);
        NodeChanged?.Invoke(this, new ValueChangedEventArgs<MixingStationNode>(node.Path, node));
    }

    public MixingStationNode? GetNode(string path)
    {
        return _state.GetNode(path);
    }

    public bool TryGetNode(string path, out MixingStationNode node)
    {
        return _state.TryGetNode(path, out node);
    }
}

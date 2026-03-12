using System;
using System.Collections.Generic;

namespace MixingStation.Api.Models;

public sealed class MixerState
{
    private readonly Dictionary<string, object?> _values = new(StringComparer.Ordinal);
    private readonly Dictionary<string, MixingStationNode> _nodes = new(StringComparer.Ordinal);

    public List<string> GetAllPaths()
    {
        return new List<string>(_values.Keys);
    }

    public bool TryGetValue<T>(string path, out T value)
    {
        if (_values.TryGetValue(path, out var raw) && raw is T typed)
        {
            value = typed;
            return true;
        }

        value = default!;
        return false;
    }

    public T? GetValue<T>(string path)
    {
        return _values.TryGetValue(path, out var raw) && raw is T typed
            ? typed
            : default;
    }

    public object? GetValue(string path)
    {
        return _values.TryGetValue(path, out var raw) ? raw : null;
    }

    public bool SetValue(string path, object? value)
    {
        if (_values.TryGetValue(path, out var existing) && Equals(existing, value))
            return false;

        _values[path] = value;
        return true;
    }

    public void SetNode(MixingStationNode node)
    {
        _nodes[node.Path] = node;
    }

    public MixingStationNode? GetNode(string path)
    {
        return _nodes.TryGetValue(path, out var node) ? node : null;
    }

    public bool TryGetNode(string path, out MixingStationNode node)
    {
        return _nodes.TryGetValue(path, out node!);
    }
}

using System;

namespace MixingStation.Api.Models;

public sealed class ValueChangedEventArgs : EventArgs
{
    public string Path { get; }
    public object? Value { get; }

    public ValueChangedEventArgs(string path, object? value)
    {
        Path = path;
        Value = value;
    }
}

public sealed class ValueChangedEventArgs<T> : EventArgs
{
    public string Path { get; }
    public T Value { get; }

    public ValueChangedEventArgs(string path, T value)
    {
        Path = path;
        Value = value;
    }
}

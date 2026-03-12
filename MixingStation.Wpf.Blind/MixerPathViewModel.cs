using MixingStation.Api.Models;
using MixingStation.Api.Services;
using System;
using System.ComponentModel;

namespace MixingStation.Api.ViewModels;

public abstract class MixerPathViewModel : INotifyPropertyChanged, IDisposable
{
    protected readonly MixerStateService State;

    public string Prefix { get; }

    protected MixerPathViewModel(string prefix, MixerStateService state)
    {
        Prefix = prefix;
        State = state;
        State.ValueChanged += HandleStateChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected T? Get<T>(string relativePath)
    {
        return State.GetValue<T>(Combine(relativePath));
    }

    protected void Set(string relativePath, object? value)
    {
        State.SetValue(Combine(relativePath), value);
    }

    protected virtual void HandleStateChanged(object? sender, ValueChangedEventArgs e)
    {
        if (e.Path.StartsWith(Prefix, StringComparison.Ordinal))
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }

    protected void Raise(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected string Combine(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return Prefix;

        return $"{Prefix}.{relativePath}";
    }

    public void Dispose()
    {
        State.ValueChanged -= HandleStateChanged;
    }
}

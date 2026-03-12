using MixingStation.Api.Attributes;
using MixingStation.Api.Helpers;
using MixingStation.Api.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MixingStation.Api.Models;

public abstract class ParameterRouter : INotifyPropertyChanged, IDisposable
{
    protected readonly MixerStateService _mixerStateService;

    private readonly Dictionary<string, string> _propertyToPath = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> _pathToProperty = new(StringComparer.Ordinal);

    private readonly DebounceTimer _debounceTimer;
    private bool _debounceTimerRunning;
    private bool _disposed;

    private readonly string? _routePrefix;
    private readonly int _channelIndex;

    public static bool LoadingFromScene = false;

    public int ChannelIndex => _channelIndex;
    public ChannelTypes ChannelType { get; }

    protected ParameterRouter(ChannelTypes channelType, int index, MixerStateService mixerStateService)
    {
        ChannelType = channelType;
        _channelIndex = index;
        _mixerStateService = mixerStateService;

        InitRouteMaps();
        _mixerStateService.ValueChanged += MixerStateChanged;

        _debounceTimer = new DebounceTimer(2000, () => _debounceTimerRunning = false);
    }

    protected ParameterRouter(string routePrefix, int index, MixerStateService mixerStateService)
    {
        _routePrefix = routePrefix;
        _channelIndex = index;
        ChannelType = ChannelTypes.NONE;
        _mixerStateService = mixerStateService;

        InitRouteMaps();
        _mixerStateService.ValueChanged += MixerStateChanged;

        _debounceTimer = new DebounceTimer(2000, () => _debounceTimerRunning = false);
    }

    public abstract event PropertyChangedEventHandler? PropertyChanged;

    public abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

    public void Dispose()
    {
        if (_disposed)
            return;

        _mixerStateService.ValueChanged -= MixerStateChanged;
        _disposed = true;
    }

    private void InitRouteMaps()
    {
        var type = GetType();
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var property in properties)
        {
            var parameterAttribute = property.GetCustomAttribute<ParameterPathAttribute>();
            var parameterName = parameterAttribute?.ParameterPath ?? property.Name;
            var path = BuildPropertyPath(parameterName);

            _propertyToPath[property.Name] = path;
            _pathToProperty[path] = property.Name;
        }
    }

    private string BuildPropertyPath(string propertyName, ChannelTypes? mixType = null, int? mixNum = null)
    {
        if (_channelIndex == -1 && ChannelType == ChannelTypes.NONE)
            return $"{_routePrefix}/{propertyName}";

        if (ChannelType == ChannelTypes.NONE)
            return $"{_routePrefix}{_channelIndex}/{propertyName}";

        return $"{ChannelUtil.GetChannelString(new ChannelSelector(ChannelType, _channelIndex, mixType, mixNum))}/{propertyName}";
    }

    private void MixerStateChanged(object? sender, ValueChangedEventArgs args)
    {
        if (_debounceTimerRunning)
            return;

        if (!_pathToProperty.TryGetValue(args.Path, out var propertyName))
            return;

        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    protected string GetPath([CallerMemberName] string propertyName = "")
    {
        return _propertyToPath[propertyName];
    }

    protected T? Get<T>([CallerMemberName] string propertyName = "")
    {
        var path = GetPath(propertyName);
        return _mixerStateService.GetValue<T>(path);
    }

    protected float GetFloat([CallerMemberName] string propertyName = "")
    {
        var path = GetPath(propertyName);
        return _mixerStateService.GetValue<float>(path);
    }

    protected string? GetString([CallerMemberName] string propertyName = "")
    {
        var path = GetPath(propertyName);
        return _mixerStateService.GetValue<string>(path);
    }

    protected string[]? GetStrings([CallerMemberName] string propertyName = "")
    {
        var path = GetPath(propertyName);
        return _mixerStateService.GetValue<string[]>(path);
    }

    protected bool GetBoolean([CallerMemberName] string propertyName = "")
    {
        var path = GetPath(propertyName);

        if (_mixerStateService.TryGetValue<bool>(path, out var boolValue))
            return boolValue;

        if (_mixerStateService.TryGetValue<float>(path, out var floatValue))
            return floatValue > 0.5f;

        return false;
    }

    protected int GetInt([CallerMemberName] string propertyName = "")
    {
        var path = GetPath(propertyName);

        if (_mixerStateService.TryGetValue<int>(path, out var intValue))
            return intValue;

        if (_mixerStateService.TryGetValue<float>(path, out var floatValue))
            return (int)floatValue;

        return 0;
    }

    protected TEnum GetEnum<TEnum>([CallerMemberName] string propertyName = "")
        where TEnum : struct, Enum
    {
        var path = GetPath(propertyName);

        if (_mixerStateService.TryGetValue<TEnum>(path, out var enumValue))
            return enumValue;

        if (_mixerStateService.TryGetValue<int>(path, out var intValue) &&
            Enum.IsDefined(typeof(TEnum), intValue))
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), intValue);
        }

        if (_mixerStateService.TryGetValue<float>(path, out var floatValue))
        {
            var enumInt = (int)floatValue;
            if (Enum.IsDefined(typeof(TEnum), enumInt))
                return (TEnum)Enum.ToObject(typeof(TEnum), enumInt);
        }

        return default;
    }

    public List<string> GetValueList(string propertyName)
    {
        var result = new List<string>();

        if (TryGetRange(propertyName, out var range))
        {
            for (var i = (int)range.Min; i <= range.Max; i++)
                result.Add((i + 1).ToString());
        }

        return result;
    }

    public bool TryGetRange(string propertyName, out Range range)
    {
        var path = _propertyToPath[propertyName];

        range = new Range();

        if (_mixerStateService.TryGetValue<float>($"{path}/max", out var max))
        {
            range.Max = max;
            _mixerStateService.TryGetValue<float>($"{path}/min", out var min);
            _mixerStateService.TryGetValue<float>($"{path}/def", out var def);

            range.Min = min;
            range.Default = def;
            return true;
        }

        return false;
    }

    protected int GetIntInRange([CallerMemberName] string propertyName = "")
    {
        var path = GetPath(propertyName);

        if (_mixerStateService.TryGetValue<float>($"{path}/max", out var max))
        {
            _mixerStateService.TryGetValue<float>($"{path}/min", out var min);

            if (min == -1)
                min = 0;

            _mixerStateService.TryGetValue<float>($"{path}/def", out _);
            var value = _mixerStateService.GetValue<float>(path);

            return (int)(max * value);
        }

        return -1;
    }

    public void SetValueFromInt(int value, [CallerMemberName] string propertyName = "")
    {
        var path = GetPath(propertyName);

        if (_mixerStateService.TryGetValue<float>($"{path}/max", out var max))
        {
            if (max <= 0)
                return;

            var result = value / max;
            BeginDebounce();
            _mixerStateService.SetValue(path, result);
        }
    }

    protected void Set(string path, object? value)
    {
        BeginDebounce();
        _mixerStateService.SetValue(path, value);
    }

    protected void SetString(string value, [CallerMemberName] string propertyName = "")
    {
        if (_propertyToPath.TryGetValue(propertyName, out var path))
            Set(path, value);
    }

    protected void SetBoolean(bool value, [CallerMemberName] string propertyName = "")
    {
        if (_propertyToPath.TryGetValue(propertyName, out var path))
            Set(path, value);
    }

    protected void SetValue(float value, [CallerMemberName] string propertyName = "")
    {
        if (_propertyToPath.TryGetValue(propertyName, out var path))
            Set(path, value);
    }

    protected void SetInt(int value, [CallerMemberName] string propertyName = "")
    {
        if (_propertyToPath.TryGetValue(propertyName, out var path))
            Set(path, value);
    }

    protected void SetEnum<TEnum>(TEnum value, [CallerMemberName] string propertyName = "")
        where TEnum : struct, Enum
    {
        if (_propertyToPath.TryGetValue(propertyName, out var path))
            Set(path, Convert.ToInt32(value));
    }

    private void BeginDebounce()
    {
        _debounceTimerRunning = true;
        _debounceTimer.Start();
    }
}

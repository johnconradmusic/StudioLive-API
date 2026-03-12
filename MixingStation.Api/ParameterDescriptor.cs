using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MixingStation.Api.Schema;

public enum ParameterKind
{
    Group,
    Number,
    Boolean,
    String,
    Unknown,
    Enum
}

public sealed class ParameterDescriptor
{
    public string Path { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;

    public ParameterKind Kind { get; init; }

    public string? Unit { get; init; }
    public double? Min { get; init; }
    public double? Max { get; init; }
    public double? Step { get; init; }

    public bool Tap { get; init; }
    public bool ReadOnly { get; init; }

    public object? DefaultValue { get; init; }
    public override string ToString()
    {
        var parts = new List<string>
    {
        $"{Path}",
        $"[{Kind}]"
    };

        if (!string.IsNullOrWhiteSpace(Title))
            parts.Add($"\"{Title}\"");

        if (Min.HasValue || Max.HasValue)
            parts.Add($"Range:{Min?.ToString() ?? "?"}..{Max?.ToString() ?? "?"}");

        if (Step.HasValue)
            parts.Add($"Step:{Step.Value}");

        if (!string.IsNullOrWhiteSpace(Unit))
            parts.Add($"Unit:{Unit.Trim()}");

        if (DefaultValue != null)
            parts.Add($"Default:{DefaultValue}");

        if (Tap)
            parts.Add("Tap");

        if (ReadOnly)
            parts.Add("ReadOnly");

        return string.Join("  ", parts);
    }
}

public sealed class UiNode : INotifyPropertyChanged
{
    private object? _currentValue;
    private bool _isReadOnly;
    private bool _isVisible = true;
    private bool _isEnabled = true;
    public string Key { get; init; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Path { get; set; }
    public ParameterKind Kind { get; set; }
    public string? Unit { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }
    public double? Step { get; set; }

    public List<UiNode> Children { get; } = new();

    public object? CurrentValue
    {
        get => _currentValue;
        set
        {
            if (Equals(_currentValue, value))
                return;

            _currentValue = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentValueText));
            OnPropertyChanged(nameof(CurrentBooleanValue));
            OnPropertyChanged(nameof(CurrentFloatValue));
            OnPropertyChanged(nameof(CurrentStringValue));
        }
    }

    public bool IsReadOnly
    {
        get => _isReadOnly;
        set
        {
            if (_isReadOnly == value)
                return;

            _isReadOnly = value;
            OnPropertyChanged();
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible == value)
                return;

            _isVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled == value)
                return;

            _isEnabled = value;
            OnPropertyChanged();
        }
    }

    public float CurrentFloatValue
    {
        get => CurrentValue switch
        {
            float f => f,
            double d => (float)d,
            int i => i,
            _ => 0f
        };
        set => CurrentValue = value;
    }

    public bool CurrentBooleanValue
    {
        get => CurrentValue switch
        {
            bool b => b,
            float f => f > 0.5f,
            double d => d > 0.5,
            int i => i != 0,
            _ => false
        };
        set => CurrentValue = value;
    }

    public string CurrentStringValue
    {
        get => CurrentValue?.ToString() ?? string.Empty;
        set => CurrentValue = value;
    }

    public string CurrentValueText
    {
        get
        {
            if (CurrentValue == null)
                return string.Empty;

            return CurrentValue switch
            {
                float f => Unit == null ? $"{f}" : $"{f}{Unit}",
                double d => Unit == null ? $"{d}" : $"{d}{Unit}",
                int i => Unit == null ? $"{i}" : $"{i}{Unit}",
                bool b => b ? "On" : "Off",
                _ => CurrentValue.ToString() ?? string.Empty
            };
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override string ToString()
    {
        var parts = new List<string>();

        parts.Add(Label);

        parts.Add($"[{Kind}]");

        if (!string.IsNullOrWhiteSpace(Path))
            parts.Add($"path:{Path}");

        if (Min.HasValue || Max.HasValue)
            parts.Add($"range:{Min?.ToString() ?? "?"}..{Max?.ToString() ?? "?"}");

        if (Step.HasValue)
            parts.Add($"step:{Step.Value}");

        if (!string.IsNullOrWhiteSpace(Unit))
            parts.Add($"unit:{Unit.Trim()}");

        if (Children.Count > 0)
            parts.Add($"children:{Children.Count}");

        return string.Join(" ", parts);
    }

    public string DumpTree(int indent = 0)
    {
        var pad = new string(' ', indent * 2);

        var line = pad + ToString();

        if (Children.Count == 0)
            return line;

        var builder = new StringBuilder();
        builder.AppendLine(line);

        foreach (var child in Children)
            builder.AppendLine(child.DumpTree(indent + 1));

        return builder.ToString().TrimEnd();
    }
}
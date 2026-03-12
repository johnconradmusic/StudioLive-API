using MixingStation.Api.Models;
using MixingStation.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace MixingStation.Api.Schema;

public sealed class UiNodeBinder : IDisposable
{
    private readonly MixerStateService _state;
    private readonly Dictionary<string, UiNode> _pathMap = new(StringComparer.Ordinal);
    private readonly Dispatcher _dispatcher;
    private bool _updatingFromState;

    public UiNodeBinder(MixerStateService state)
    {
        _state = state;
        _dispatcher = Application.Current?.Dispatcher
            ?? Dispatcher.CurrentDispatcher;

        _state.ValueChanged += HandleStateChanged;
    }

    public void BindTree(UiNode root)
    {
        _pathMap.Clear();
        IndexNode(root);

        foreach (var pair in _pathMap)
        {
            var value = _state.GetValue(pair.Key);
            if (value != null)
                pair.Value.CurrentValue = value;

            pair.Value.PropertyChanged += HandleNodePropertyChanged;
        }
    }

    private void IndexNode(UiNode node)
    {
        if (!string.IsNullOrWhiteSpace(node.Path))
            _pathMap[node.Path] = node;

        foreach (var child in node.Children)
            IndexNode(child);
    }

    private void HandleStateChanged(object? sender, ValueChangedEventArgs e)
    {
        if (!_pathMap.TryGetValue(e.Path, out var node))
            return;

        void Apply()
        {
            _updatingFromState = true;
            try
            {
                node.CurrentValue = e.Value;
            }
            finally
            {
                _updatingFromState = false;
            }
        }

        if (_dispatcher.CheckAccess())
            Apply();
        else
            _dispatcher.BeginInvoke((Action)Apply);
    }

    private void HandleNodePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_updatingFromState)
            return;

        if (e.PropertyName != nameof(UiNode.CurrentValue))
            return;

        if (sender is not UiNode node || string.IsNullOrWhiteSpace(node.Path))
            return;

        _state.SetValue(node.Path, node.CurrentValue);
    }

    public void Dispose()
    {
        _state.ValueChanged -= HandleStateChanged;

        foreach (var node in _pathMap.Values)
            node.PropertyChanged -= HandleNodePropertyChanged;

        _pathMap.Clear();
    }
}
using MixingStation.Api.Models;
using MixingStation.Api.Services;
using System;
using System.ComponentModel;
using System.Windows;

namespace MixingStation.Api.ViewModels;

public sealed class ChannelViewModel : MixerPathViewModel
{
    public int Index { get; }

    public ChannelViewModel(int index, MixerStateService state)
        : base($"ch.{index}", state)
    {
        Index = index;
    }

    public string? Name
    {
        get => Get<string>("name");
        set => Set("name", value);
    }

    public string? UserName
    {
        get => Get<string>("username");
        set => Set("username", value);
    }

    public string? Color
    {
        get => Get<string>("color");
        set => Set("color", value);
    }

    public bool Select
    {
        get => Get<bool>("select");
        set => Set("select", value);
    }

    public bool Solo
    {
        get => Get<bool>("solo");
        set => Set("solo", value);
    }

    public bool Mute
    {
        get => Get<bool>("mute");
        set => Set("mute", value);
    }

    public float Volume
    {
        get => Get<float>("mix.lvl");
        set => Set("mix.lvl", value);
    }

    public float Pan
    {
        get => Get<float>("pan");
        set => Set("pan", value);
    }

    public bool Link
    {
        get => Get<bool>("link");
        set
        {
            Set("link", value);
            Raise(nameof(LinkSlave));
            Raise(nameof(LinkedVisibility));
        }
    }

    public bool LinkMaster
    {
        get => Get<bool>("linkmaster");
        set
        {
            Set("linkmaster", value);
            Raise(nameof(LinkSlave));
            Raise(nameof(LinkedVisibility));
        }
    }

    public bool Linkable => Index % 2 != 0;

    public bool LinkSlave => Link && !LinkMaster;

    public Visibility LinkedVisibility =>
        LinkMaster ? Visibility.Visible :
        Link && !LinkMaster ? Visibility.Collapsed :
        Visibility.Visible;

    protected override void HandleStateChanged(object? sender, ValueChangedEventArgs e)
    {
        if (!e.Path.StartsWith(Prefix, StringComparison.Ordinal))
            return;

        base.HandleStateChanged(sender, e);

        if (e.Path == $"{Prefix}.link" || e.Path == $"{Prefix}.linkmaster")
        {
            Raise(nameof(LinkSlave));
            Raise(nameof(LinkedVisibility));
        }
    }
}

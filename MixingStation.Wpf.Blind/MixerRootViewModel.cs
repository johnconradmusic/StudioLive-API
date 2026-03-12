using MixingStation.Api.Services;
using System.Collections.ObjectModel;

namespace MixingStation.Api.ViewModels;

public sealed class MixerRootViewModel
{
    public ObservableCollection<ChannelViewModel> Channels { get; } = new();
    public ObservableCollection<FxViewModel> Fx { get; } = new();
    public ObservableCollection<MuteGroupViewModel> MuteGroups { get; } = new();

    public RoutingViewModel Routing { get; }
    public RtaViewModel Rta { get; }

    public MixerRootViewModel(MixerStateService state, int channelCount, int fxCount, int muteGroupCount)
    {
        for (var i = 0; i < channelCount; i++)
            Channels.Add(new ChannelViewModel(i, state));

        for (var i = 0; i < fxCount; i++)
            Fx.Add(new FxViewModel(i, state));

        for (var i = 0; i < muteGroupCount; i++)
            MuteGroups.Add(new MuteGroupViewModel(i, state));

        Routing = new RoutingViewModel(state);
        Rta = new RtaViewModel(state);
    }
}

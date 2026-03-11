using System;

namespace MixingStation.Api.Models;

public class MixingStationMixerEndpoint
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Host { get; set; } = "127.0.0.1";
    public int RestPort { get; set; }
    public int WebSocketPort { get; set; }

    public Uri RestBaseUri => new($"http://{Host}:{RestPort}");

    public Uri WebSocketUri => new($"ws://{Host}:{WebSocketPort}/ws");
}

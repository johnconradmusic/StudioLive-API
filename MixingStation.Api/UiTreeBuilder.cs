using System;
using System.Collections.Generic;
using System.Linq;

namespace MixingStation.Api.Schema;

public sealed class UiTreeBuilder
{
    public UiNode Build(IEnumerable<ParameterDescriptor> descriptors)
    {
        var root = new UiNode
        {
            Key = "root",
            Label = "Mixer",
            Kind = ParameterKind.Group
        };

        foreach (var descriptor in descriptors.OrderBy(d => d.Path, StringComparer.Ordinal))
        {
            AddDescriptor(root, descriptor);
        }

        return root;
    }

    private static void AddDescriptor(UiNode root, ParameterDescriptor descriptor)
    {
        var segments = descriptor.Path.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
            return;

        UiNode current = root;
        var partial = new List<string>();

        for (var i = 0; i < segments.Length; i++)
        {
            partial.Add(segments[i]);
            var key = string.Join(".", partial);
            var isLeaf = i == segments.Length - 1;

            var existing = current.Children.FirstOrDefault(c => c.Key == key);
            if (existing != null)
            {
                current = existing;
                continue;
            }

            UiNode next;
            if (isLeaf)
            {
                next = new UiNode
                {
                    Key = key,
                    Label = descriptor.Title,
                    Path = descriptor.Path,
                    Kind = descriptor.Kind,
                    Unit = descriptor.Unit,
                    Min = descriptor.Min,
                    Max = descriptor.Max,
                    Step = descriptor.Step
                };
            }
            else
            {
                next = new UiNode
                {
                    Key = key,
                    Label = FormatGroupLabel(segments[i], i, segments),
                    Kind = ParameterKind.Group
                };
            }

            current.Children.Add(next);
            current = next;
        }
    }

    private static string FormatGroupLabel(string segment, int index, string[] segments)
    {
        if (index > 0 && int.TryParse(segment, out var numericIndex))
        {
            var parent = segments[index - 1];
            return parent switch
            {
                "ch" => $"Channel {numericIndex + 1}",
                "fx" => $"FX {numericIndex + 1}",
                "muteGroups" => $"Mute Group {numericIndex + 1}",
                _ => $"{parent} {numericIndex + 1}"
            };
        }

        return segment switch
        {
            "ch" => "Channels",
            "fx" => "FX",
            "muteGroups" => "Mute Groups",
            "routing" => "Routing",
            "rta" => "RTA",
            _ => segment
        };
    }
}

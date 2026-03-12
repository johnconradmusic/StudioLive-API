using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MixingStation.Api.Schema;

public sealed class MixingStationSchemaLoader
{
    private readonly HttpClient _httpClient;

    public MixingStationSchemaLoader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<ParameterDescriptor?> LoadDefinitionAsync(string path, CancellationToken cancellationToken = default)
    {
        if(path.Contains("lvl"))
        {

        }
        using var response = await _httpClient.GetAsync($"console/data/definitions2/{path}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(json))
            return null;
        using var doc = JsonDocument.Parse(json);
        return ParseDefinition(path, doc.RootElement);
    }

    public async Task<List<ParameterDescriptor>> LoadAllDefinitionsAsync(
        IEnumerable<string> paths,
        CancellationToken cancellationToken = default)
    {
        var results = new List<ParameterDescriptor>();

        foreach (var path in paths)
        {
            try
            {
                var descriptor = await LoadDefinitionAsync(path, cancellationToken).ConfigureAwait(false);
                if (descriptor != null)
                    results.Add(descriptor);
            }
            catch (Exception ex)
            {
               // Log.Warning(ex, "Failed to load definition for path {Path}", path);
            }
        }

        return results;
    }

    private static ParameterDescriptor? ParseDefinition(string path, JsonElement root)
    {
        if (root.TryGetProperty("node", out var el)) // This is a node, that will contain children
        {
            return new ParameterDescriptor
            {
                Path = path,
                Name = GetLeafName(path),
                Title = TryGetString(root, "title") ?? GetLeafName(path),
                Kind = ParameterKind.Group,
                ReadOnly = true
            };
        }

        if (!root.TryGetProperty("value", out var valueElement) )
            return null;

        var title = TryGetString(valueElement, "title") ?? GetLeafName(path);
        var type = TryGetString(valueElement, "type");

        var desc = new ParameterDescriptor
        {
            Path = path,
            Name = GetLeafName(path),
            Title = title,
            Kind = MapKind(type),
            Unit = TryGetString(valueElement, "unit"),
            Min = TryGetDouble(valueElement, "min"),
            Max = TryGetDouble(valueElement, "max"),
            Step = TryGetDouble(valueElement, "delta"),
            Tap = TryGetBool(valueElement, "tap"),
            ReadOnly = false
        };

        //Log.Debug("Parsed parameter descriptor: {Descriptor}", desc);

        return desc;
    }

    private static ParameterKind MapKind(string? type)
    {
        return type?.ToLowerInvariant() switch
        {
            "float" => ParameterKind.Number,
            "double" => ParameterKind.Number,
            "int" => ParameterKind.Number,
            "integer" => ParameterKind.Number,
            "bool" => ParameterKind.Boolean,
            "boolean" => ParameterKind.Boolean,
            "string" => ParameterKind.String,
            "enum" => ParameterKind.Enum,
            _ => ParameterKind.Unknown
        };
    }

    private static string GetLeafName(string path)
    {
        var lastDot = path.LastIndexOf('.');
        return lastDot >= 0 ? path[(lastDot + 1)..] : path;
    }

    private static string? TryGetString(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String
            ? prop.GetString()
            : null;
    }

    private static double? TryGetDouble(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.Number)
        {
            if (prop.TryGetDouble(out var value))
                return value;
        }

        return null;
    }

    private static bool TryGetBool(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var prop) &&
               (prop.ValueKind == JsonValueKind.True || prop.ValueKind == JsonValueKind.False) &&
               prop.GetBoolean();
    }
}
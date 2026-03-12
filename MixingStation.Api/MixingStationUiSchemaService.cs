using MixingStation.Api.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MixingStation.Api.Schema;

public sealed class MixingStationUiSchemaService
{
    private readonly MixingStationSchemaLoader _loader;
    private readonly UiTreeBuilder _treeBuilder;

    public MixingStationUiSchemaService(MixingStationSchemaLoader loader, UiTreeBuilder treeBuilder)
    {
        _loader = loader;
        _treeBuilder = treeBuilder;
    }

    public async Task<UiNode> BuildUiTreeAsync(List<string> paths, CancellationToken cancellationToken = default)
    {
        var descriptors = await _loader.LoadAllDefinitionsAsync(paths, cancellationToken).ConfigureAwait(false);
        return _treeBuilder.Build(descriptors);
    }
}
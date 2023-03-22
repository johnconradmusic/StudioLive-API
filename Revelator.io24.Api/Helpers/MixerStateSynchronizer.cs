using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using System.Text.Json;

namespace Presonus.UCNet.Api;
public class MixerStateSynchronizer
{
	private readonly MixerStateTraverser _traverser;

	public MixerStateSynchronizer(MixerStateTraverser traverser)
	{
		_traverser = traverser;
	}

	public void Synchronize(string json, MixerState mixerState)
	{
		var doc = JsonSerializer.Deserialize<JsonDocument>(json);
		if (doc == null) return;

		var children = doc.RootElement;
		_traverser.Traverse(children, string.Empty, mixerState);
	}
}

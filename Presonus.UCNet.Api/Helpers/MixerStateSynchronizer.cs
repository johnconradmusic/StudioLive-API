using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Services;
using System;
using System.IO;
using System.Text.Json;

namespace Presonus.UCNet.Api;
public class MixerStateSynchronizer
{
	private readonly MixerStateTraverser _traverser;

	public MixerStateSynchronizer(MixerStateTraverser traverser)
	{
		_traverser = traverser;
	}

	public void Synchronize(string json, MixerStateService mixerState)
	{
		var doc = JsonSerializer.Deserialize<JsonDocument>(json);
		if (doc == null) return;

		if (doc.RootElement.TryGetProperty("children", out var children))
		{
			_traverser.Traverse(children, string.Empty, mixerState);
			Mixer.Counted = true;

		}
	}


}

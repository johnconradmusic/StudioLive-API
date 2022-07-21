using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Presonus.UC.Api.Components.Parameters;

namespace Presonus.StudioLive32.Api.Console;

public partial class MixingConsole
{
	public List<Channel> line { get; set; } = new List<Channel>();
	public Mastersection mastersection { get; set; }
	public MixingConsole()
	{
		CreateMasterSection();
		CreateChannels(32);
	}

	private void CreateMasterSection()
	{
		mastersection = new Mastersection();
		mastersection.Address = "mastersection/";

	}




	public void CreateChannels(int num)
	{
		for (int i = 0; i < num; i++)
		{
			var chan = new MicLineInput() { Address = $"line/ch{i + 1}/"};
			line.Add(chan);
		}
	}
}
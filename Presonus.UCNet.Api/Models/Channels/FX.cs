using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Channels
{
	public class FX : ParameterRouter
	{
		public static Dictionary<string, string> FXNames = new Dictionary<string, string>()
		{
			{"{1F85A838-3073-4AA5-9D2A-4E7CF93854EB}", "Mono Delay"},
			{"{384DA04C-150F-7244-292A-7FF880EE4AED}", "Stereo Delay"},
			{"{365B9BD6-2047-32EC-9DCB-6B2DE1938A98}", "Ping Pong Delay"},
			{"{18C4D8AE-4556-AD74-778C-D68D5CD017ED}", "Standard Reverb"},
			{"{7E836A3A-9E90-4C09-9F2C-0A9C06255770}", "PAE16 Reverb"},
			{"{02DEEFCA-C4F9-F49A-EE82-20298A86190B}", "PAE335 Reverb"},
			{"{C1CD3BB7-48E7-AB2D-1F67-B1C78FBA87D9}", "Vintage Reverb"},
			{"{5D80F75E-628E-FF6F-9B7B-BCB8174A997B}", "Digital XL Reverb"},
			{"{E61B3D46-3FD4-4F13-9F04-37643ED7BBB5}", "Chorus"},
			{"{BE30005C-1209-AEF7-8764-F616C1CF46CF}", "Flanger" }
		};
		public FX(int index, MixerStateService mixerStateService) : base("fx/ch", index, mixerStateService)
		{

		}

		[ParameterPath("plugin/classId")]
		public string classId { get => GetString(); set => SetString(value); }

		public string Name => FXNames[classId];

		public override event PropertyChangedEventHandler PropertyChanged;

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}

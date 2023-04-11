using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.NewDataModel;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class Presets : ParameterRouter, INotifyPropertyChanged
	{
		public Presets(MixerStateService mixerStateService) : base(ChannelTypes.NONE, -1, mixerStateService)
		{
		}

		[ParameterPath("presets/loaded_scene_name")]
		public string CurrentScene
		{
			get
			{
				string filePath = GetString();

				// Split the file path by the directory separator character
				string[] pathParts = filePath.Split('/');

				// Extract the project and scene names
				string projectName = pathParts.Length > 1 ? pathParts[1] : string.Empty;
				string sceneName = pathParts.Length > 2 ? Path.GetFileNameWithoutExtension(pathParts[2]) : string.Empty;

				// Regular expression to match the number and a dot followed by a space at the beginning of the name
				Regex regex = new Regex(@"^(\d+)\.");

				// Extract the save slot numbers from the project and scene names
				int projectSaveSlot = int.Parse(regex.Match(projectName).Groups[1].Value);
				int sceneSaveSlot = int.Parse(regex.Match(sceneName).Groups[1].Value);

				// Remove the number from the project and scene names
				projectName = regex.Replace(projectName, "");
				sceneName = regex.Replace(sceneName, "");

				// Display the project and scene names along with their save slots
				Console.WriteLine($"Project Name: {projectName} (Save Slot: {projectSaveSlot})");
				Console.WriteLine($"Scene Name: {sceneName} (Save Slot: {sceneSaveSlot})");

				return $"{projectName.ToUpper()}: {sceneName.ToUpper()}";
			}

		}

		public override event PropertyChangedEventHandler PropertyChanged;

		protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}

	}
}

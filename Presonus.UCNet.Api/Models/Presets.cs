using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class Presets : ParameterRouter, INotifyPropertyChanged
	{
		public enum Operation
		{
			StoreScene, RecallScene, StoreChannel, RecallChannel, StoreProject, RecallProject
		}
		public Presets(MixerStateService mixerStateService) : base("presets", -1, mixerStateService)
		{
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		[ParameterPath("loaded_scene_name")]
		public string LoadedSceneName { get => GetString(); set => SetString(value); }

		[ParameterPath("loaded_scene_title")]
		public string LoadedSceneTitle { get => GetString(); set => SetString(value); }

		[ParameterPath("loaded_project_name")]
		public string LoadedProjectName { get => GetString(); set => SetString(value); }

		[ParameterPath("loaded_project_title")]
		public string LoadedProjectTitle { get => GetString(); set => SetString(value); }

		[ParameterPath("loading_scene")]
		public bool LoadingScene { get => GetBoolean(); set => SetBoolean(value); }

		public async Task<List<GenericListItem>> GetProjects() => await _mixerStateService.GetProjects();

		public async Task<List<GenericListItem>> GetScenes() => await _mixerStateService.GetScenes(LoadedProjectName);


		public void FileOperation(Operation operation, string projFile = "", string sceneFile = "", ChannelSelector selector = null)
		{
			if (operation == Operation.StoreScene || operation == Operation.RecallScene) projFile = LoadedProjectName;
			_mixerStateService.FileOperationMethod(operation, projFile, sceneFile, selector);
		}

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}

		public async Task<List<GenericListItem>> GetChannelPresets()
		{
			return await _mixerStateService.GetPresets();
		}
	}
}
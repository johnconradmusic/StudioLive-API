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
		public enum OperationType
		{
			StoreScene, RecallScene, StoreChannel, RecallChannel, StoreProject, RecallProject, ResetScene, ResetProject, ResetChannel, CopyChannel, PasteChannel
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

		public void ChannelCopyPaste(ChannelSelector channel, bool paste)
		{
			_mixerStateService.ChannelCopyPaste(channel, paste);
		}

		public void ResetChannel(ChannelTypes channelTypes, int index)
		{
			_mixerStateService.ChannelResetMethod(channelTypes, index);
		}

		public void FileOperation(OperationType operation, string projFile = "", string sceneFile = "", ChannelSelector selector = null)
		{
			if (operation == OperationType.StoreScene || operation == OperationType.RecallScene) projFile = LoadedProjectName;
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
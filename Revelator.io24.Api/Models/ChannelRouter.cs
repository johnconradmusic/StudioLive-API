using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.NewDataModel;
using Presonus.UCNet.Api.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Presonus.StudioLive32.Api.Models
{
	public abstract class ChannelRouter : INotifyPropertyChanged
	{
		private readonly MixerStateService _mixerStateService;

		private ChannelTypes _channelType;
		private int _channelIndex;
		public static bool loadingFromScene = false;
		public readonly string _routePrefix;

		public ChannelRouter(ChannelTypes channelType, int index, MixerStateService mixerStateService)
		{
			_channelIndex = index;
			_channelType = channelType;
			_mixerStateService = mixerStateService;
			_mixerStateService.Synchronized += Synchronized;
			_mixerStateService.ValueChanged += ValueStateUpdated;
			_mixerStateService.StringChanged += StringStateUpdated;
			_mixerStateService.StringsChanged += StringsStateUpdated;
		}

		public abstract event PropertyChangedEventHandler PropertyChanged;

		private string GetPropertyPath(string propertyName, ChannelTypes? mixType = null, int? mixNum = null)
		{
			return ChannelUtil.GetChannelString(new(_channelType, _channelIndex, mixType, mixNum)) + $"/{propertyName}";
		}

		private void ValueStateUpdated(object sender, ValueChangedEventArgs<float> e)
		{
			//Console.WriteLine("value updated " + e.Path);
			OnPropertyChanged(new PropertyChangedEventArgs(e.Path.Split("/").Last()));
		}

		private void StringStateUpdated(object sender, ValueChangedEventArgs<string> e)
		{
			//Console.WriteLine("string updated " + e.Path);
			OnPropertyChanged(new PropertyChangedEventArgs(e.Path.Split("/").Last()));
		}

		private void StringsStateUpdated(object sender, ValueChangedEventArgs<string[]> e)
		{
			//Console.WriteLine("value updated " + e.ToString());
			OnPropertyChanged(new PropertyChangedEventArgs(e.Path.Split("/").Last()));
		}

		protected abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

		protected string GetString([CallerMemberName] string propertyName = "")
		{
			return _mixerStateService.GetString(GetPropertyPath(propertyName));
		}

		protected bool GetBoolean([CallerMemberName] string propertyName = "")
		{
			var value = _mixerStateService.GetValue(GetPropertyPath(propertyName));
			return value > 0.5f;
		}

		protected float GetValue([CallerMemberName] string propertyName = "", bool useRange = true)
		{
			return _mixerStateService.GetValue(GetPropertyPath(propertyName));
		}

		protected void SetString(string value, [CallerMemberName] string propertyName = "")
		{
			_mixerStateService.SetString(GetPropertyPath(propertyName), value);
		}

		protected void SetBoolean(bool value, [CallerMemberName] string propertyName = "")
		{
			var floatValue = value ? 1.0f : 0.0f;
			_mixerStateService.SetValue(GetPropertyPath(propertyName), floatValue);
			//OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		protected void SetValue(float value, [CallerMemberName] string propertyName = "", bool useRange = true)
		{
			_mixerStateService.SetValue(GetPropertyPath(propertyName), value);
			//OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		public void Synchronized(object sender, EventArgs e)
		{
			var type = this.GetType();
			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				OnPropertyChanged(new PropertyChangedEventArgs(property.Name));
			}
		}
	}
}
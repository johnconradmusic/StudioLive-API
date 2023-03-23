using Presonus.StudioLive32.Api.Attributes;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.NewDataModel;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Presonus.StudioLive32.Api.Models
{
	public abstract class ChannelRouter : INotifyPropertyChanged
	{
		private readonly MixerStateService _mixerStateService;

		private ChannelTypes _channelType;
		private int _channelIndex;
		public static bool loadingFromScene = false;
		private Dictionary<string, string> _propertyValueNameRoute = new();


		public ChannelRouter(ChannelTypes channelType, int index, MixerStateService mixerStateService)
		{
			_channelIndex = index;
			_channelType = channelType;
			_mixerStateService = mixerStateService;
			_mixerStateService.Synchronized += Synchronized;
			_mixerStateService.ValueChanged += ValueStateUpdated;
			_mixerStateService.StringChanged += StringStateUpdated;
			_mixerStateService.StringsChanged += StringsStateUpdated;

			InitMapRoutes();
		}

		public abstract event PropertyChangedEventHandler PropertyChanged;
		private void InitMapRoutes()
		{
			var type = this.GetType();

			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				if (property is null)
					continue;

				var parameterName = property.GetCustomAttribute<ParameterPathAttribute>();
				if (parameterName != null && (property.PropertyType == typeof(bool) || property.PropertyType == typeof(string) || property.PropertyType == typeof(string[]) || property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType.IsEnum))
				{
					_propertyValueNameRoute[GetPropertyPath(parameterName.ParameterPath)] = property.Name;
				}
			}
		}
		private string GetPropertyPath(string propertyName, ChannelTypes? mixType = null, int? mixNum = null)
		{
			return ChannelUtil.GetChannelString(new(_channelType, _channelIndex, mixType, mixNum)) + $"/{propertyName}";
		}

		private void ValueStateUpdated(object sender, ValueChangedEventArgs<float> e)
		{
			//OnPropertyChanged(new PropertyChangedEventArgs(_propertyValueNameRoute[e.Path]));
		}

		private void StringStateUpdated(object sender, ValueChangedEventArgs<string> e)
		{
			//OnPropertyChanged(new PropertyChangedEventArgs(_propertyValueNameRoute[e.Path]));
		}

		private void StringsStateUpdated(object sender, ValueChangedEventArgs<string[]> e)
		{
			//OnPropertyChanged(new PropertyChangedEventArgs(_propertyValueNameRoute[e.Path]));
		}

		protected abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

		protected string GetString([CallerMemberName] string propertyName = "")
		{
			return _mixerStateService.GetString(GetPropertyPath(propertyName.ToLower()));
		}

		protected bool GetBoolean([CallerMemberName] string propertyName = "")
		{
			var value = _mixerStateService.GetValue(GetPropertyPath(propertyName.ToLower()));
			return value > 0.5f;
		}

		protected float GetValue([CallerMemberName] string propertyName = "", bool useRange = true)
		{
			return _mixerStateService.GetValue(GetPropertyPath(propertyName.ToLower()));
		}

		protected void SetString(string value, [CallerMemberName] string propertyName = "")
		{
			_mixerStateService.SetString(GetPropertyPath(propertyName.ToLower()), value);
		}

		protected void SetBoolean(bool value, [CallerMemberName] string propertyName = "")
		{
			var floatValue = value ? 1.0f : 0.0f;
			_mixerStateService.SetValue(GetPropertyPath(propertyName.ToLower()), floatValue);
		}

		protected void SetValue(float value, [CallerMemberName] string propertyName = "", bool useRange = true)
		{
			_mixerStateService.SetValue(GetPropertyPath(propertyName.ToLower()), value);
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
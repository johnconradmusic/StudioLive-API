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
	public abstract class ParameterRouter : INotifyPropertyChanged
	{
		private readonly MixerStateService _mixerStateService;

		private ChannelTypes _channelType;
		private int _channelIndex;
		public static bool loadingFromScene = false;
		private Dictionary<string, string> _propertyValueNameRoute = new();


		public ParameterRouter(ChannelTypes channelType, int index, MixerStateService mixerStateService)
		{
			_channelIndex = index;
			_channelType = channelType;
			_mixerStateService = mixerStateService;
			_mixerStateService.ValueChanged += ValueStateUpdated;
			_mixerStateService.StringChanged += StringStateUpdated;
			_mixerStateService.StringsChanged += StringsStateUpdated;

			InitMapRoutes();
		}
		protected abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

		public abstract event PropertyChangedEventHandler PropertyChanged;
		private void InitMapRoutes()
		{
			var type = this.GetType();

			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				if (property is null)
					continue;

				var parameterAttribute = property.GetCustomAttribute<ParameterPathAttribute>();
				string parameterName = parameterAttribute == null ? property.Name : parameterAttribute.ParameterPath;
				if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(string) || property.PropertyType == typeof(string[]) || property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType.IsEnum)
				{
					_propertyValueNameRoute[property.Name] = GetPropertyPath(parameterName);
				}
			}
		}
		private string GetPropertyPath(string propertyName, ChannelTypes? mixType = null, int? mixNum = null)
		{
			return ChannelUtil.GetChannelString(new(_channelType, _channelIndex, mixType, mixNum)) + $"/{propertyName}";
		}

		private void ValueStateUpdated(object sender, ValueChangedEventArgs<float> args)
		{
			var propertyName = _propertyValueNameRoute.SingleOrDefault(pair => pair.Value == args.Path).Key;
			if (propertyName is null)
				return;

			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		private void StringStateUpdated(object sender, ValueChangedEventArgs<string> args)
		{

			var propertyName = _propertyValueNameRoute.SingleOrDefault(pair => pair.Value == args.Path).Key;
			if (propertyName is null)
			{
				return;
			}
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		private void StringsStateUpdated(object sender, ValueChangedEventArgs<string[]> args)
		{
			var propertyName = _propertyValueNameRoute.SingleOrDefault(pair => pair.Value == args.Path).Key;
			if (propertyName is null)
				return;

			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}


		protected string GetString([CallerMemberName] string propertyName = "")
		{
			var path = _propertyValueNameRoute[propertyName];
			var value = _mixerStateService.GetString(path);

			return value;
		}

		protected bool GetBoolean([CallerMemberName] string propertyName = "")
		{
			var path = _propertyValueNameRoute[propertyName];
			var value = _mixerStateService.GetValue(path);
			var result = value > 0.5f;
			return result;
		}

		protected int GetIntInRange([CallerMemberName] string propertyName = "")
		{
			var path = _propertyValueNameRoute[propertyName];
			var value = _mixerStateService.GetValue(path);

			if (_mixerStateService.TryGetValue(path + "/max", out float max)) //inclusive, (zero-based
			{
				_mixerStateService.TryGetValue(path + "/min", out float min);
				if (min == -1) min = 0;
				_mixerStateService.TryGetValue(path + "/def", out float def);

				var result = (int)(max * value);
				Console.WriteLine($"Get Int in range {propertyName} {result}");
				return result;
			}
			return -1;

		}

		public void SetValueFromInt(int value, [CallerMemberName] string propertyName = "")
		{
			var path = _propertyValueNameRoute[propertyName];

			if (_mixerStateService.TryGetValue(path + "/max", out float max)) //inclusive, (zero-based
			{
				_mixerStateService.TryGetValue(path + "/min", out float min);
				if (min == -1) min = 0;
				_mixerStateService.TryGetValue(path + "/def", out float def);

				var result = (float)(value / max);
				Console.WriteLine($"Get Int in range {propertyName} {result}");
				_mixerStateService.SetValue(path, result);
			}
		}

		protected float GetValue([CallerMemberName] string propertyName = "")
		{
			var path = _propertyValueNameRoute[propertyName];
			var value = _mixerStateService.GetValue(path);
			return value;
		}

		protected void SetString(string value, [CallerMemberName] string propertyName = "")
		{
			if (_propertyValueNameRoute.TryGetValue(propertyName, out var pathName))
				_mixerStateService.SetString(pathName, value);
		}

		protected void SetBoolean(bool value, [CallerMemberName] string propertyName = "")
		{
			var floatValue = value ? 1.0f : 0.0f;
			if (_propertyValueNameRoute.TryGetValue(propertyName, out var pathName))
				_mixerStateService.SetValue(pathName, floatValue);
		}

		protected void SetValue(float value, [CallerMemberName] string propertyName = "")
		{
			if (_propertyValueNameRoute.TryGetValue(propertyName, out var pathName))
				_mixerStateService.SetValue(pathName, value);
		}
	}
}
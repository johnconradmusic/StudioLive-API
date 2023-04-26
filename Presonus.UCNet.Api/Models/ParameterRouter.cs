using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;

using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Presonus.UCNet.Api.Models
{
	public abstract class ParameterRouter : INotifyPropertyChanged
	{
		private readonly MixerStateService _mixerStateService;

		private ChannelTypes channelType;
		private int _channelIndex;
		public static bool loadingFromScene = false;
		private Dictionary<string, string> _propertyValueNameRoute = new();
		private Dictionary<string, string> _propertyStringNameRoute = new();
		private Dictionary<string, string> _propertyStringsNameRoute = new();

		private readonly DebounceTimer _debounceTimer;
		private bool _debounceTimerRunning;

		public int ChannelIndex { get => _channelIndex; }
		public ChannelTypes ChannelType { get => channelType; set => channelType = value; }

		public ParameterRouter(ChannelTypes channelType, int index, MixerStateService mixerStateService)
		{
			_channelIndex = index;
			ChannelType = channelType;
			_mixerStateService = mixerStateService;
			_mixerStateService.ValueChanged += ValueStateUpdated;
			_mixerStateService.StringChanged += StringStateUpdated;
			_mixerStateService.StringsChanged += StringsStateUpdated;

			InitMapRoutes();

			_debounceTimer = new(2000, () => _debounceTimerRunning = false);
		}


		public abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

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
				if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType.IsEnum)
				{
					_propertyValueNameRoute[property.Name] = GetPropertyPath(parameterName);
				}
				if (property.PropertyType == typeof(string))
				{
					_propertyStringNameRoute[property.Name] = GetPropertyPath(parameterName);
				}
				if (property.PropertyType == typeof(string[]))
				{
					_propertyStringsNameRoute[property.Name] = GetPropertyPath(parameterName);
				}
			}
		}
		private string GetPropertyPath(string propertyName, ChannelTypes? mixType = null, int? mixNum = null)
		{
			if (ChannelType == ChannelTypes.NONE) return propertyName;
			return ChannelUtil.GetChannelString(new(ChannelType, _channelIndex, mixType, mixNum)) + $"/{propertyName}";
		}

		private void ValueStateUpdated(object sender, ValueChangedEventArgs<float> args)
		{
			if (_debounceTimerRunning) return;
			var propertyName = _propertyValueNameRoute.SingleOrDefault(pair => pair.Value == args.Path).Key;
			if (propertyName is null)
				return;

			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		private void StringStateUpdated(object sender, ValueChangedEventArgs<string> args)
		{
			if (_debounceTimerRunning) return;

			var propertyName = _propertyStringNameRoute.SingleOrDefault(pair => pair.Value == args.Path).Key;

			if (propertyName is null)
			{
				return;
			}
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		private void StringsStateUpdated(object sender, ValueChangedEventArgs<string[]> args)
		{
			if (_debounceTimerRunning) return;

			var propertyName = _propertyStringsNameRoute.SingleOrDefault(pair => pair.Value == args.Path).Key;
			if (propertyName is null)
				return;

			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}


		protected string GetString([CallerMemberName] string propertyName = "")
		{
			var path = _propertyStringNameRoute[propertyName];
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

		public List<string> GetValueList(string propertyName)
		{
			List<string> result = new();
			if (TryGetRange(propertyName, out var range))
			{
				for (int i = (int)range.Min; i <= range.Max; i++)
				{
					result.Add((i + 1).ToString());
				}
			}
			return result;
		}

		public bool TryGetRange(string propertyName, out UCNet.Api.Models.Range range)
		{
			var path = _propertyValueNameRoute[propertyName];
			range = new();
			if (_mixerStateService.TryGetValue(path + "/max", out float max)) //inclusive, (zero-based)
			{
				range.Max = max;
				_mixerStateService.TryGetValue(path + "/min", out float min);
				range.Min = min;
				_mixerStateService.TryGetValue(path + "/def", out float def);
				range.Default = def;
				return true;
			}
			return false;
		}

		protected int GetIntInRange([CallerMemberName] string propertyName = "")
		{
			var path = _propertyValueNameRoute[propertyName];
			var value = _mixerStateService.GetValue(path);

			if (_mixerStateService.TryGetValue(path + "/max", out float max)) //inclusive, (zero-based)
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

			if (_mixerStateService.TryGetValue(path + "/max", out float max)) //inclusive, (zero-based)
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
			_debounceTimerRunning = true;
			_debounceTimer.Start();

			if (_propertyStringNameRoute.TryGetValue(propertyName, out var pathName))
				_mixerStateService.SetString(pathName, value);
		}

		protected void SetBoolean(bool value, [CallerMemberName] string propertyName = "")
		{
			_debounceTimerRunning = true;
			_debounceTimer.Start();
			var floatValue = value ? 1.0f : 0.0f;
			if (_propertyValueNameRoute.TryGetValue(propertyName, out var pathName))
				_mixerStateService.SetValue(pathName, floatValue);
		}

		protected void SetValue(float value, [CallerMemberName] string propertyName = "")
		{
			_debounceTimerRunning = true;
			_debounceTimer.Start();
			if (_propertyValueNameRoute.TryGetValue(propertyName, out var pathName))
				_mixerStateService.SetValue(pathName, value);
		}
	}
}
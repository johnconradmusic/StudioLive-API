﻿using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Models.Inputs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Revelator.io24.Api.Models
{
	public abstract class DeviceRoutingBase
	{
		private readonly RawService _rawService;

		private readonly Dictionary<string, string> _propertyValueNameRoute = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _propertyStringNameRoute = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _propertyStringsNameRoute = new Dictionary<string, string>();

		private readonly string _routePrefix;

		public DeviceRoutingBase(string routePrefix, RawService rawService)
		{
			_rawService = rawService;
			_routePrefix = routePrefix;
			_rawService.Syncronized += Syncronized;
			_rawService.ValueStateUpdated += ValueStateUpdated;
			_rawService.StringStateUpdated += StringStateUpdated;
			_rawService.StringsStateUpdated += StringsStateUpdated;
			InitMapRoutes();
		}

		protected abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

		//TODO: Add GetStringRoute and GetStringsRoute? Could be refactored to be isolated away from each other.
		public string GetValueRoute([CallerMemberName] string propertyName = "")
			=> _propertyValueNameRoute.TryGetValue(propertyName, out var route)
				? route
				: default;

		private void Syncronized()
		{
			var type = this.GetType();
			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				OnPropertyChanged(new PropertyChangedEventArgs(property.Name));
			}
		}

		private void ValueStateUpdated(string route, float value)
		{
			var propertyName = _propertyValueNameRoute.SingleOrDefault(pair => pair.Value == route).Key;
			if (propertyName is null)
				return;

			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		private void StringStateUpdated(string route, string value)
		{
			var propertyName = _propertyStringNameRoute.SingleOrDefault(pair => pair.Value == route).Key;
			if (propertyName is null)
				return;

			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		private void StringsStateUpdated(string route, string[] value)
		{
			var propertyName = _propertyStringsNameRoute.SingleOrDefault(pair => pair.Value == route).Key;
			if (propertyName is null)
				return;

			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		private void InitMapRoutes()
		{
			var type = this.GetType();

			if (_routePrefix is null)
				return;

			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				if (property is null)
					continue;

				var routeValue = property.GetCustomAttribute<RouteValueAttribute>();
				if (routeValue != null || property.PropertyType == typeof(int) || property.PropertyType == typeof(double))
				{
					var route = routeValue != null ? $"{_routePrefix}/{routeValue.RouteValueName}" : $"{_routePrefix}/{property.Name}";
					_propertyValueNameRoute[property.Name] = route;
					continue;
				}

				var routeString = property.GetCustomAttribute<RouteStringAttribute>();
				if (routeString != null || property.PropertyType == typeof(string))
				{
					var route = routeString != null ? $"{_routePrefix}/{routeString.RouteStringName}" : $"{_routePrefix}/{property.Name}";
					_propertyStringNameRoute[property.Name] = route;
					continue;
				}

				var routeStrings = property.GetCustomAttribute<RouteStringsAttribute>();
				if (routeStrings != null)
				{
					var route = $"{_routePrefix}/{routeStrings.RouteStringsName}";
					_propertyStringsNameRoute[property.Name] = route;
					continue;
				}

			}
		}

		protected string[] GetStrings([CallerMemberName] string propertyName = "")
		{
			if (!_propertyStringsNameRoute.TryGetValue(propertyName, out var route))
				return Array.Empty<string>();

			return _rawService.GetStrings(route);
		}

		protected string GetString([CallerMemberName] string propertyName = "")
		{
			if (!_propertyStringNameRoute.TryGetValue(propertyName, out var route))
				return default;

			return _rawService.GetString(route);
		}

		protected void SetString(string value, [CallerMemberName] string propertyName = "")
		{
			Serilog.Log.Information("set string " + propertyName + " - " + value);
			if (value is null)
				return;

			if (!_propertyStringNameRoute.TryGetValue(propertyName, out var route))
				return;

			Serilog.Log.Information("route? " + route);


			_rawService.SetString(route, value);
		}

		protected void SetBoolean(bool value, [CallerMemberName] string propertyName = "")
		{

			foreach (var property in _propertyValueNameRoute) Serilog.Log.Debug(property.ToString());
			if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
				return;

			var floatValue = value ? 1.0f : 0.0f;
			_rawService.SetValue(route, floatValue);
		}

		protected bool GetBoolean([CallerMemberName] string propertyName = "")
		{
			if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
				return default;

			var value = _rawService.GetValue(route);
			return value > 0.5f;
		}

		protected double GetValue([CallerMemberName] string propertyName = "")
		{
			if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
				return default;

			return _rawService.GetValue(route);

		}

		protected void SetValue(double value, [CallerMemberName] string propertyName = "")
		{
			if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
				return;

			_rawService.SetValue(route, value);
		}

		protected int GetVolume([CallerMemberName] string propertyName = "")
		{
			if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
				return default;

			var floatValue = _rawService.GetValue(route);
			return (int)Math.Round(floatValue * 100f);
		}

		protected void SetVolume(double value, [CallerMemberName] string propertyName = "")
		{
			if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
				return;

			var floatValue = value / 100f;
			_rawService.SetValue(route, floatValue);
		}
	}
}

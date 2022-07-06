﻿using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models.Inputs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Presonus.StudioLive32.Api.Models
{
    [Serializable]
    public abstract class DeviceRoutingBase : INotifyPropertyChanged
    {
        private readonly RawService _rawService;

        private readonly Dictionary<string, string> _propertyValueNameRoute = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _propertyStringNameRoute = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _propertyStringsNameRoute = new Dictionary<string, string>();

        protected readonly string _routePrefix;

        public abstract event PropertyChangedEventHandler PropertyChanged;

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

        public DeviceRoutingBase() { }

        protected abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

        //TODO: Add GetStringRoute and GetStringsRoute? Could be refactored to be isolated away from each other.
        public string GetValueRoute([CallerMemberName] string propertyName = "")
            => _propertyValueNameRoute.TryGetValue(propertyName, out var route)
                ? route
                : default;

        public void Syncronized()
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
                if (routeValue != null || property.PropertyType == typeof(bool) || property.PropertyType == typeof(int) || property.PropertyType == typeof(float))
                {
                    var route = routeValue != null ? $"{_routePrefix}/{routeValue.RouteValueName}" : $"{_routePrefix}/{property.Name}";
                    _propertyValueNameRoute[property.Name.ToLower()] = route;
                    var range = property.GetCustomAttribute<RouteValueRangeAttribute>();
                    if (range != null)
                    {
                        _rawService._propertyValueRanges[route] = new ValueRange(range.Min, range.Max, range.Unit);
                    }
                    continue;
                }

                var routeString = property.GetCustomAttribute<RouteStringAttribute>();
                if (routeString != null || property.PropertyType == typeof(string))
                {
                    var route = routeString != null ? $"{_routePrefix}/{routeString.RouteStringName}" : $"{_routePrefix}/{property.Name}";
                    _propertyStringNameRoute[property.Name.ToLower()] = route;
                    continue;
                }

                var routeStrings = property.GetCustomAttribute<RouteStringsAttribute>();
                if (routeStrings != null)
                {
                    var route = $"{_routePrefix}/{routeStrings.RouteStringsName}";
                    _propertyStringsNameRoute[property.Name.ToLower()] = route;
                    continue;
                }

            }
        }

        protected string GetString([CallerMemberName] string propertyName = "")
        {
            if (!_propertyStringNameRoute.TryGetValue(propertyName, out var route))
                return default;

            return _rawService.GetString(route);
        }

        protected void SetString(string value, [CallerMemberName] string propertyName = "")
        {
            if (value is null)
                return;

            if (!_propertyStringNameRoute.TryGetValue(propertyName, out var route))
                return;
            Console.WriteLine("Device routing base set string: value:" + value);
            _rawService.SetString(route, value);
        }

        protected void SetBoolean(bool value, [CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return;

            var floatValue = value ? 1.0f : 0.0f;
            _rawService.SetValue(route, floatValue);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        }

        protected bool GetBoolean([CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return default;

            var value = _rawService.GetValue(route);
            return value > 0.5f;
        }

        protected float GetValue([CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return default;

            _rawService._propertyValueRanges.TryGetValue(route, out var range);

            var value = _rawService.GetValue(route);
            if (range != null)
            {
                var topOfRange = range.Max - range.Min;
                value = (value * topOfRange) + range.Min;
            }
            return value;
        }

        protected void SetValue(float value, [CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return;
            _rawService._propertyValueRanges.TryGetValue(route, out var range);

            if (range != null)
            {
                var topOfRange = range.Max - range.Min;
                value = (value - range.Min) / topOfRange;

            }

            _rawService.SetValue(route, value);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected int GetVolume([CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return default;

            var floatValue = _rawService.GetValue(route);
            return (int)Math.Round(floatValue * 100f);
        }

        protected void SetVolume(float value, [CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return;

            var floatValue = value / 100f;
            _rawService.SetValue(route, floatValue);
        }
    }
}

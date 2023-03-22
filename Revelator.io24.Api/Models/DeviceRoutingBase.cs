using Newtonsoft.Json;
using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models.Inputs;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Presonus.StudioLive32.Api.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public abstract class DeviceRoutingBase : INotifyPropertyChanged
    {
        [JsonIgnore] private readonly MixerStateService _mixerStateService;

        public readonly Dictionary<string, string> _propertyValueNameRoute = new Dictionary<string, string>();
        public readonly Dictionary<string, string> _propertyStringNameRoute = new Dictionary<string, string>();
        public readonly Dictionary<string, string> _propertyStringsNameRoute = new Dictionary<string, string>();

        public readonly string _routePrefix;

        public abstract event PropertyChangedEventHandler PropertyChanged;

        public static bool loadingFromScene = false;
        public DeviceRoutingBase(string routePrefix, MixerStateService rawService)
        {            
            _mixerStateService = rawService;
            _routePrefix = routePrefix;
            _mixerStateService.Synchronized += Synchronized;
            _mixerStateService.ValueChanged += ValueStateUpdated;
            _mixerStateService.StringChanged += StringStateUpdated;
            _mixerStateService.StringsChanged += StringsStateUpdated;
            InitMapRoutes();
        }

        protected abstract void OnPropertyChanged(PropertyChangedEventArgs eventArgs);

        public string GetValueRoute([CallerMemberName] string propertyName = "")
            => _propertyValueNameRoute.TryGetValue(propertyName, out var route)
                ? route
                : default;

        public void Synchronized(object sender, EventArgs e)
        {
            var type = this.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(property.Name));
            }
        }

        private void ValueStateUpdated(object sender, ValueChangedEventArgs<float> e)
        {
            var propertyName = _propertyValueNameRoute.SingleOrDefault(pair => pair.Value == e.Path).Key;
            if (propertyName is null)
                return;
            Console.WriteLine("value updated "+ e.ToString());
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void StringStateUpdated(object sender, ValueChangedEventArgs<string> e)
        {
            var propertyName = _propertyStringNameRoute.SingleOrDefault(pair => pair.Value == e.Path).Key;
            if (propertyName is null)
                return;

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void StringsStateUpdated(object sender, ValueChangedEventArgs<string[]> e)
        {
            var propertyName = _propertyStringsNameRoute.SingleOrDefault(pair => pair.Value == e.Path).Key;
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
                if (routeValue != null || property.PropertyType == typeof(bool) || property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType.IsEnum)
                {
                    var route = routeValue != null ? $"{_routePrefix}/{routeValue.RouteValueName}" : $"{_routePrefix}/{property.Name}";

                    _propertyValueNameRoute[property.Name.ToLower()] = route;
                    var range = property.GetCustomAttribute<RouteValueRangeAttribute>();
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

            return _mixerStateService.GetString(route);
        }

        protected void SetString(string value, [CallerMemberName] string propertyName = "")
        {
            if (value is null)
                return;

            //Console.WriteLine("Device routing base set string: value:" + propertyName + " : " + value);
            if (!_propertyStringNameRoute.TryGetValue(propertyName, out var route))
                return;
            _mixerStateService.SetString(route, value);
        }

        protected void SetBoolean(bool value, [CallerMemberName] string propertyName = "")
        {
            //Console.WriteLine(propertyName + ": " + value);
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return;
            var floatValue = value ? 1.0f : 0.0f;
            _mixerStateService.SetValue(route, floatValue);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        }

        protected bool GetBoolean([CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return default;

            var value = _mixerStateService.GetValue(route);
            return value > 0.5f;
        }

        protected float GetVolume([CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return default;

            var floatValue = _mixerStateService.GetValue(route);
            //if (loadingFromScene) return floatValue;
            return Util.GetDBFromFloat(floatValue);
        }

        protected void SetVolume(float value, [CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return;

            var floatValue = Util.GetFloatFromDB(value);
            //if (loadingFromScene) floatValue = value;
            _mixerStateService.SetValue(route, floatValue);
        }

        protected float GetFrequency([CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return default;

            var floatValue = _mixerStateService.GetValue(route);
            //if (loadingFromScene) return floatValue;

            return Util.GetFrequencyFromFloat(floatValue);
        }

        protected void SetFrequency(float value, [CallerMemberName] string propertyName = "")
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return;

            var floatValue = Util.GetFloatFromFrequency(value);
            //if (loadingFromScene) floatValue = value;

            _mixerStateService.SetValue(route, floatValue);
        }
        protected float GetValue([CallerMemberName] string propertyName = "", bool useRange = true)
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return default;

            var value = _mixerStateService.GetValue(route);
            
            return value;
        }

        protected T GetEnumValue<T>([CallerMemberName] string propertyName = "") where T : Enum
        {
            var length = Enum.GetNames(typeof(T)).Length - 1;
            IList<T> enumVals = (IList<T>)Enum.GetValues(typeof(T));
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return default;

            var value = _mixerStateService.GetValue(route);


            if (Enum.IsDefined(typeof(T), 0))
            {//is zero-based
             //if (value < 1 && value > 0)//must be a float 
                return enumVals[(int)(value * length)];
                //else 
                //return enumVals[(int)value];
            }
            else
            {
                //if (value > 1) return enumVals[(int)value];
                //else
                //{
                if (Enum.IsDefined(typeof(T), 1)) length++;
                var res = enumVals[(int)(value * length)];
                return res;
                //}
            }

        }

        protected void SetEnumValue<T>(T value, [CallerMemberName] string propertyName = "") where T : Enum
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return;

            var length = Enum.GetNames(typeof(T)).Length;
            IList<T> enumVals = (IList<T>)Enum.GetValues(typeof(T));

            float enumVal = 0;

            if (Enum.IsDefined(typeof(T), 0))
            {//is zero-based?
                enumVal = enumVals.IndexOf(value);
                length--;
            }
            else
            {
                enumVal = enumVals.IndexOf(value) + 1;
            }

            float floatValue = enumVal / length;

            _mixerStateService.SetValue(route, floatValue);
        }

        protected void SetValue(float value, [CallerMemberName] string propertyName = "", bool useRange = true)
        {
            if (!_propertyValueNameRoute.TryGetValue(propertyName, out var route))
                return;
            
            _mixerStateService.SetValue(route, value);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}

using Presonus.StudioLive32.Api.Models;
using Presonus.UC.Api;
using Presonus.UC.Api.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace Presonus.StudioLive32.Api
{
    public class RawService
    {
        public delegate void SyncronizeEvent();
        public delegate void ValueStateEvent(string route, float value);
        public delegate void StringStateEvent(string route, string value);
        public delegate void StringsStateEvent(string route, string[] value);
        public class SceneFile
        {
            public Dictionary<string, float> _values = new Dictionary<string, float>();
            public Dictionary<string, string> _string = new Dictionary<string, string>();
            public Dictionary<string, string[]> _strings = new Dictionary<string, string[]>();

            public SceneFile(Dictionary<string, float> values, Dictionary<string, string> @string, Dictionary<string, string[]> strings)
            {
                _values = values;
                _string = @string;
                _strings = strings;
            }
        }

        private Dictionary<string, float> _values = new Dictionary<string, float>();
        private Dictionary<string, string> _string = new Dictionary<string, string>();
        private Dictionary<string, string[]> _strings = new Dictionary<string, string[]>();
        internal Dictionary<string, ValueRange> _propertyValueRanges = new Dictionary<string, ValueRange>();

        public event SyncronizeEvent Syncronized;
        public event ValueStateEvent ValueStateUpdated;
        public event StringStateEvent StringStateUpdated;
        public event StringsStateEvent StringsStateUpdated;

        internal Action<string, string, bool> SetStringMethod;
        internal Action<string, float, bool> SetValueMethod;

        public static RawService Instance;

        public bool ConnectionEstablished;

        public void TryLoadScene(string path)
        {
            //Dictionary<string, float> newValues = (Dictionary<string, float>)Serializer.Deserialize("C:\\Dev\\Scenes\\testScene.scn", typeof(Dictionary<string, float>));
            SceneFile loadedScene = (SceneFile)Serializer.Deserialize(path, typeof(SceneFile));
            if (loadedScene == null) return;
            foreach (KeyValuePair<string, float> kvp in loadedScene._values)
            {
                if (kvp.Key.Contains("src"))
                {
                    //continue;
                }
                if (!_values.ContainsKey(kvp.Key) || _values[kvp.Key] != kvp.Value)
                {
                    SetValue(kvp.Key, kvp.Value);
                    Thread.Sleep(10);
                }
            }
            _values = loadedScene._values;
            foreach (KeyValuePair<string, string> kvp in loadedScene._string)
            {
                if (!_string.ContainsKey(kvp.Key) || _string[kvp.Key] != kvp.Value)
                {
                    SetString(kvp.Key, kvp.Value);
                    Thread.Sleep(10);
                }
            }
            _string = loadedScene._string;
            Syncronized?.Invoke();
        }

        public void TrySaveScene(string path)
        {
            Serializer.Serialize(new SceneFile(_values, _string, _strings), path);
        }

        public void SetString(string route, string value)
        {
            if (route is null)
                return;

            if (_string.TryGetValue(route, out var oldValue) && oldValue == value)
            {
                //Console.WriteLine("string value was the same, skipping..." + route + " : " + value);
                return;
            }
            //Console.WriteLine("set string: " + route + " : " + value);
            SetStringMethod?.Invoke(route, value, true);
        }

        public void SetValue(string route, float value)
        {
            if (route is null)
                return;

            if (DeviceRoutingBase.loadingFromScene && route.Contains("meter")) return;

            var firstIndex = route.IndexOf("aux");
            var result = firstIndex != route.LastIndexOf("aux") && firstIndex != -1;
            if (result) return;

            //Check if value has actually changed:

            //if (_values.TryGetValue(route, out var oldValue))
            //{
            //    if (oldValue == value)
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    //Serilog.Log.Information("Why didn't this route contain a value?!? " + route);
            //}
            //Thread.Sleep(10);

            //TODO: Refactor... We will need to split listen and send for that to work.
            SetValueMethod?.Invoke(route, value, true);
        }

        public float GetValue(string route)
        {
            var success = _values.TryGetValue(route, out var value);
            if (success)
            {
                return value;
            }
            else
                return default;
        }

        public string GetString(string route)
        {
            //Serilog.Log.Information("GET: " + route);
            return _string.TryGetValue(route, out var value)
                           ? value : default;
        }

        public string[] GetStrings(string route)
            => _strings.TryGetValue(route, out var value)
                ? value : Array.Empty<string>();

        internal void UpdateValueState(string route, float value)
        {
            if (!route.Contains("meter"))
                Serilog.Log.Information("update value state: " + route + ": " + value.ToString());
            _values[route] = value;
            ValueStateUpdated?.Invoke(route, value);
        }

        internal void UpdateStringState(string route, string value)
        {
            //Serilog.Log.Information("update string state: " + route + ": " + value.ToString());
            _string[route] = value;
            StringStateUpdated?.Invoke(route, value);
        }

        internal void UpdateStringsState(string route, string[] values)
        {

            //Serilog.Log.Information("update strings state");
            _strings[route] = values;
            StringsStateUpdated?.Invoke(route, values);
        }

        internal void Syncronize(string json)
        {
            //Serilog.Log.Information("Synchronize");
            //Serilog.Log.Warning(json);
            var doc = JsonSerializer.Deserialize<JsonDocument>(json);
            if (doc is null)
                return;

            var id = doc.RootElement.GetProperty("id").GetString();
            var children = doc.RootElement.GetProperty("children");
            var shared = doc.RootElement
                .GetProperty("shared")
                .GetProperty("strings")
                .EnumerateArray()
                .First()
                .EnumerateArray()
                .Select(item => item.GetString())
                .ToArray();

            Traverse(children, string.Empty);

            Syncronized?.Invoke();
        }
        internal void SyncronizeState(string json)
        {
            Serilog.Log.Information("SynchronizeState");
            //Serilog.Log.Warning(json);
            var doc = JsonSerializer.Deserialize<JsonDocument>(json);
            if (doc is null)
                return;

            var root = doc.RootElement;


            TraverseSceneFile(root, string.Empty);

            Syncronized?.Invoke();
        }

        public void JSON()
        {
            //var sceneFile = File.ReadAllText("C:\\Dev\\new-scene.scn");
            //SyncronizeState(sceneFile);
        }
        private void Traverse(JsonElement element, string path)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Number:
                    var firstIndex = path.IndexOf("aux");
                    var result = firstIndex != path.LastIndexOf("aux") && firstIndex != -1;
                    if (result) return;
                    _values[path] = element.GetSingle();

                    if (path.Contains("src")) return;
                    //Serilog.Log.Information(path + ": " + element.GetSingle().ToString());

                    return;
                case JsonValueKind.String:
                    _string[path] = element.GetString() ?? string.Empty;
                    return;
                case JsonValueKind.Array:
                    var array = element.EnumerateArray();
                    _strings[path] = array
                        .Select(item => item.GetString() ?? string.Empty)
                        .Where(str => str != string.Empty)
                        .ToArray();
                    return;
                case JsonValueKind.Object:
                    TraverseObject(element, path);
                    return;
                default:
                    //TODO: Logging, what is going on here?
                    return;
            }
        }

        private void TraverseSceneFile(JsonElement element, string path)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Number:

                    var value = element.GetSingle();
                    //Serilog.Log.Information(path + ": " + value.ToString());
                    var firstIndex = path.IndexOf("aux");
                    var result = firstIndex != path.LastIndexOf("aux") && firstIndex != -1;
                    if (result) return;
                    _propertyValueRanges.TryGetValue(path, out var range);

                    if (path.Contains("src"))
                    {

                    }

                    if (path.Contains("freq"))
                    {
                        value = Util.GetFloatFromFrequency(value);
                        _values[path] = value;
                        return;
                    }
                    if (path.Contains("volume") || (path.Contains("aux") && !path.Contains("pan") && !path.Contains("flags") && !path.StartsWith("aux")))
                        value = Util.GetFloatFromDB(value);
                    if (range != null)
                    {
                        var topOfRange = range.Max - range.Min;
                        value = (value - range.Min) / topOfRange;
                    }
                    _values[path] = value;
                    //SetValue(path, value);
                    return;
                case JsonValueKind.String:
                    var strVal = element.GetString();
                    _string[path] = strVal ?? string.Empty;
                    //Serilog.Log.Information(path + ": " + strVal);
                    return;
                case JsonValueKind.Array:
                    var array = element.EnumerateArray();
                    _strings[path] = array
                        .Select(item => item.GetString() ?? string.Empty)
                        .Where(str => str != string.Empty)
                        .ToArray();
                    return;
                case JsonValueKind.Object:
                    TraverseSceneObject(element, path);
                    return;
                default:
                    //TODO: Logging, what is going on here?
                    return;
            }
        }
        private void TraverseSceneObject(JsonElement objectElement, string path)
        {
            var properties = objectElement.EnumerateObject();
            foreach (var property in properties)
            {
                switch (property.Name)
                {
                    //Theese we can get from the ValueKind, should just be passed up with no '/' added.
                    case "children":
                    case "values":
                    case "ranges":
                    case "strings":
                        TraverseSceneFile(property.Value, CreatePath(path));
                        continue;
                    default:
                        var pathName = CreatePath(path, property.Name);
                        TraverseSceneFile(property.Value, pathName);
                        continue;
                }
            }
        }
        private void TraverseObject(JsonElement objectElement, string path)
        {
            var properties = objectElement.EnumerateObject();
            foreach (var property in properties)
            {
                switch (property.Name)
                {
                    //Theese we can get from the ValueKind, should just be passed up with no '/' added.
                    case "children":
                    case "values":
                    case "ranges":
                    case "strings":
                        Traverse(property.Value, CreatePath(path));
                        continue;
                    default:
                        var pathName = CreatePath(path, property.Name);
                        Traverse(property.Value, pathName);
                        continue;
                }
            }
        }

        private string CreatePath(string path, string propertyName = null)
        {
            //Path should never start with a '/'
            if (path is null || path.Length < 1)
                return $"{path}{propertyName}";

            //Path already ends with '/', should not add another one
            if (path.Last() == '/')
                return $"{path}{propertyName}";

            //Add '/' to path:
            return $"{path}/{propertyName}";
        }


    }
}

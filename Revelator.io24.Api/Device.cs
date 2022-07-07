using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Auxes;
using Presonus.StudioLive32.Api.Models.Inputs;
using Presonus.StudioLive32.Api.Models.Outputs;
using Presonus.UC.Api.Sound;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Presonus.StudioLive32.Api
{
    [Serializable]
    public class Device : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs) { PropertyChanged?.Invoke(this, eventArgs); }

        public bool AutoClipAvoidance { get; set; } = true;

        public void SetStateFromLoadedSceneFile(string jsonString)
        {
            Console.WriteLine("load scene");

            if (jsonString == null) return;

            var doc = JsonSerializer.Deserialize<JsonDocument>(jsonString);

            var root = doc.RootElement;

            TraverseSceneFile(root, "");

        }

        private void TraverseSceneFile(JsonElement element, string path)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Number:
                    var value = element.GetSingle();
                    Console.WriteLine(path + " - " + value);
                    //_values[path] = value;
                    //SetValue(path, value);
                    return;
                case JsonValueKind.String:
                    var strVal = element.GetString();
                    Console.WriteLine(path + " - " + strVal);

                    //_string[path] = strVal ?? string.Empty;
                    //Serilog.Log.Information(path + ": " + strVal);
                    return;
                case JsonValueKind.Array:
                    var array = element.EnumerateArray();
                    
                    //Console.WriteLine("THIS IS AN ARRAY");
                    int index = 0;
                    foreach (var channel in array)
                    {
                        //Console.WriteLine(item.ToString());
                        var channelProps = channel.EnumerateObject();
                        foreach (var property in channelProps)
                        {                            
                                Channels[index].GetType().GetProperty(property.Name).SetValue(Channels[index], property.Value);
                        }
                        index++;
                    }
                    //_strings[path] = array
                    //    .Select(item => item.GetString() ?? string.Empty)
                    //    .Where(str => str != string.Empty)
                    //    .ToArray();
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
                //Console.WriteLine(property.Name + " - "  + property.Value);
                switch (property.Name)
                {
                    //Theese we can get from the ValueKind, should just be passed up with no '/' added.
                    case "Buses":
                        return;
                    case "Channels":
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
        public bool IsAnyChannelClipping
        {
            get
            {
                return Channels.Any((c) => c.level_meter > -3);
            }
        }
        [NonSerialized]
        private readonly RawService _rawService;

        public RawService RawService => _rawService;
        public List<ChannelBase> Channels { get; set; } = new List<ChannelBase>();
        public List<BusChannel> Buses { get; set; } = new List<BusChannel>();
        public readonly int ChannelCount = 32;
        public readonly int ReturnCount = 3;
        public readonly int AuxCount = 16;
        public readonly int FXReturnCount = 4;
        public Device(RawService rawService)
        {
            _rawService = rawService;

            for (int i = 0; i < ChannelCount; i++)
            {
                var chan = new LineChannel("line/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
                chan.PropertyChanged += Chan_PropertyChanged;
            }
            for (int i = 0; i < FXReturnCount; i++)
            {
                var chan = new ReturnChannel("fxreturn/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
            }
            for (int i = 0; i < ReturnCount; i++)
            {
                var chan = new ReturnChannel("return/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
            }
            for (int i = 0; i < AuxCount; i++)
            {
                var chan = new BusChannel("aux/ch" + (i + 1).ToString(), rawService);
                Channels.Add(chan);
                Buses.Add(chan);
            }
            var main = new BusChannel("main/ch1", rawService);

            Channels.Add(main);
            Buses.Add(main);
            //start timer for clip check


        }


        private void CheckClips()
        {
            if (!AutoClipAvoidance) return;
            Console.WriteLine("check clips");
            if (IsAnyChannelClipping)
            {
                foreach (var chan in Channels)
                {
                    if (chan.level_meter > -3 || chan.clip)
                    {
                        if (chan is LineChannel lineChannel)
                        {
                            lineChannel.AutoAdjustTrim();
                        }
                    }
                }
            }
        }

        private void Chan_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "level_meter")
            {
                if (IsAnyChannelClipping)
                {
                    SoundPlayer.PlaySound("clip.wav");
                    CheckClips();
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAnyChannelClipping)));
            }
            OnPropertyChanged(e);
        }
    }
}

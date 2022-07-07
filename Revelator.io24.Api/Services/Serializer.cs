using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Presonus.StudioLive32.Api;

namespace Presonus.UC.Api.Services
{
    public class Serializer
    {
        public static void Serialize(object obj)
        {
            var jsonString = JsonSerializer.Serialize(obj, obj.GetType(), new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText("C:\\Dev\\scenefile.scene", jsonString);
        }

        public static string Deserialize(string v)
        {
            return File.ReadAllText(v);
            //return JsonSerializer.Deserialize(jsonString, typeof(Device));
        }
    }
}

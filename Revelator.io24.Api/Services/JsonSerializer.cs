using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace Presonus.UC.Api;

public class Serializer
{
    public static object Deserialize(string path, Type type)
    {
        try
        {
            //Information("Deserializing " + path);

            if (!File.Exists(path)) return null;

            var jsonString = File.ReadAllText(path);
            var obj = JsonConvert.DeserializeObject(jsonString, type, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });

            return obj;
        }
        catch (Exception e)
        {
            Serilog.Log.Error(e.Message);
            return null;
        }
    }

    public static void Serialize(object obj, string path)
    {
        Serilog.Log.Information("Serializing " + path);
        JsonSerializer serializer = new JsonSerializer();
        serializer.TypeNameHandling = TypeNameHandling.All;
        serializer.Formatting = Formatting.Indented;
        serializer.NullValueHandling = NullValueHandling.Ignore;
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using StreamWriter sw = new StreamWriter(path);
        using JsonWriter writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, obj);


    }

    protected static bool IsFileLocked(string path)
    {
        try
        {
            FileInfo info = new(path);
            using var stream = info.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }
}

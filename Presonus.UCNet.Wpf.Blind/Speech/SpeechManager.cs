using System;
using System.Linq;

namespace Presonus.UCNet.Wpf.Blind.Speech;

public class SpeechManager
{
    private static SpeechManager _instance;
    private readonly IAccessibleOutput[] _outputs;

    public SpeechManager()
    {
        _instance ??= this;
        _outputs = new IAccessibleOutput[] { new NvdaOutput(), new SapiOutput() };
    }

    public IAccessibleOutput ScreenReader => GetFirstAvailableOutput();

    private IAccessibleOutput GetFirstAvailableOutput() => _outputs.FirstOrDefault(x => x.IsAvailable());

    public static void Silence()
    {
        _instance.ScreenReader.StopSpeaking();
    }

    public static void Say(object obj, bool interrupt = true)
    {
        if (obj == null || string.IsNullOrEmpty(obj.ToString()))
        {
            return;
        }
        var message = obj.ToString()!;
        Console.WriteLine("Say " + message);
        _instance.ScreenReader.Speak(message, interrupt);
    }
}
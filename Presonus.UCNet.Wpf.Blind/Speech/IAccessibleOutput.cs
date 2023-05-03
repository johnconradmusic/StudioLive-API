namespace Presonus.UCNet.Wpf.Blind.Speech
{
    public interface IAccessibleOutput
    {
        int Rate { get; set; }

        bool IsAvailable();

        void Speak(string text);

        void Speak(string text, bool interrupt);

        void StopSpeaking();
    }
}
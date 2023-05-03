using System.Speech.Synthesis;

namespace Presonus.UCNet.Wpf.Blind.Speech
{
    public class SapiOutput : IAccessibleOutput
    {
        private SpeechSynthesizer synth;

        public SapiOutput()
        {
            synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
            synth.Rate = 5;
        }

        public int Rate { get => synth.Rate; set => synth.Rate = value; }

        public bool IsAvailable()
        {
            return true;
        }

        public void Speak(string text)
        {
            Speak(text, false);
        }

        public void Speak(string text, bool interrupt)
        {
            if (interrupt)
            {
                StopSpeaking();
            }

            synth.SpeakAsync(text);
        }

        public void StopSpeaking()
        {
            synth.SpeakAsyncCancelAll();
        }
    }
}
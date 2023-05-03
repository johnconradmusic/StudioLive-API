namespace Presonus.UCNet.Wpf.Blind.Speech
{
    using System.Linq;

    public class AutoOutput : IAccessibleOutput
    {
        private static readonly IAccessibleOutput[] outputs = { new NvdaOutput(), new SapiOutput() };

        private int rate = 5;

        public AutoOutput()
        {
        }

        public int Rate
        {
            get
            {
                return rate;
            }
            set
            {
                rate = value;
                foreach (var output in outputs)
                    if (output is SapiOutput sapi) sapi.Rate = value;
            }
        }

        private IAccessibleOutput GetFirstAvailableOutput()
        {
            return outputs.FirstOrDefault(x => x.IsAvailable());
        }

        public bool IsAvailable()
        {
            return outputs.Any(x => x.IsAvailable());
        }

        public void Speak(string text)
        {
            Speak(text, false);
        }

        public void Speak(string text, bool interrupt)
        {
            IAccessibleOutput output = GetFirstAvailableOutput();

            if (interrupt)
            {
                output.StopSpeaking();
            }

            output.Speak(text);
        }

        public void StopSpeaking()
        {
            IAccessibleOutput output = GetFirstAvailableOutput();
            output.StopSpeaking();
        }
    }
}
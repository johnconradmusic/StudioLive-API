namespace Presonus.UCNet.Wpf.Blind.Speech
{
    using System;
    using System.Runtime.InteropServices;

    public class NvdaOutput : IAccessibleOutput
    {
        public NvdaOutput()
        {
        }

        public int Rate { get => 1; set => value = 1; }

        public bool IsAvailable()
        {
            if (Environment.Is64BitProcess)
            {
                return NativeMethods64.nvdaController_testIfRunning() == 0;
            }
            else
            {
                return NativeMethods32.nvdaController_testIfRunning() == 0;
            }
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

            if (Environment.Is64BitProcess)
            {
                NativeMethods64.nvdaController_speakText(text);
            }
            else
            {
                NativeMethods32.nvdaController_speakText(text);
            }
        }

        public void StopSpeaking()
        {
            if (Environment.Is64BitProcess)
            {
                NativeMethods64.nvdaController_cancelSpeech();
            }
            else
            {
                NativeMethods32.nvdaController_cancelSpeech();
            }
        }

        internal static class NativeMethods32
        {
            [DllImport("./Libraries/nvdaControllerClient32.dll")]
            internal static extern int nvdaController_cancelSpeech();

            [DllImport("./Libraries/nvdaControllerClient32.dll", CharSet = CharSet.Auto)]
            internal static extern int nvdaController_speakText(string text);

            [DllImport("./Libraries/nvdaControllerClient32.dll")]
            internal static extern int nvdaController_testIfRunning();
        }

        internal static class NativeMethods64
        {
            [DllImport("./Libraries/nvdaControllerClient64.dll")]
            internal static extern int nvdaController_cancelSpeech();

            [DllImport("./Libraries/nvdaControllerClient64.dll", CharSet = CharSet.Auto)]
            internal static extern int nvdaController_speakText(string text);

            [DllImport("./Libraries/nvdaControllerClient64.dll")]
            internal static extern int nvdaController_testIfRunning();
        }
    }
}
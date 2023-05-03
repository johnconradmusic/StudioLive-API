using System.Runtime.InteropServices;

namespace Presonus.UCNet.Wpf.Blind.UserControls
{
    public static class ModifierKeys
    {
        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        public static bool IsAltDown()
        {
            return GetKeyState(0x12) < 0;
        }

        public static bool IsCtrlDown()
        {
            return GetKeyState(0x11) < 0;
        }

        public static bool IsShiftDown()
        {
            return GetKeyState(0x10) < 0;
        }
    }
}
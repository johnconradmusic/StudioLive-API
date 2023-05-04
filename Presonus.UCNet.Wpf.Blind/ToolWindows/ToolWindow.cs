using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
    public class ToolWindow : Window
    {
        public ToolWindow(Channel channel)
        {
            DataContext = channel;
            PreviewKeyDown += ToolWindow_PreviewKeyDown;
            WindowStyle = WindowStyle.ToolWindow;
            Loaded += ToolWindow_Loaded;
        }

        private void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Speech.SpeechManager.Say(Title + " - press escape to close.");
        }

        private void ToolWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                e.Handled = true;
                Close();
            }
        }
    }
}

using Presonus.UCNet.Api.Models;

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
    /// <summary>
    /// Interaction logic for GateWindow.xaml
    /// </summary>
    public partial class GateToolWindow : ToolWindow
    {
        public GateToolWindow(Channel channel) : base(channel)
        {
            InitializeComponent();
            Title = $"Gate Window - {channel.chnum} ({channel.username})";

        }
    }
}
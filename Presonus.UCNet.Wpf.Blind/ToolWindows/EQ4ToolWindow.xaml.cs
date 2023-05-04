using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
    /// <summary>
    /// Interaction logic for EQ4ToolWindow.xaml
    /// </summary>
    public partial class EQ4ToolWindow : ToolWindow
    {
        public EQ4ToolWindow(Channel channel) : base(channel)
        {
            InitializeComponent();
            Title = $"EQ Window - {channel.chnum} ({channel.username})";
        }
    }
}

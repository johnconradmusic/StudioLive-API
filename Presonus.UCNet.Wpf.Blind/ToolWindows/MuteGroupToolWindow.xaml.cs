using Presonus.UCNet.Api;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Wpf.Blind.UserControls;
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
    /// Interaction logic for MuteGroupToolWindow.xaml
    /// </summary>
    public partial class MuteGroupToolWindow : ToolWindow
    {

        List<bool> mutes { get; set; } = new();
        BlindViewModel viewModel;

        public MuteGroupToolWindow(int index, Mutegroup mutegroup, BlindViewModel blindViewModel)
        {
            InitializeComponent();
            Title = $"Mute Group {index} Assigns";
            Loaded += MuteGroupToolWindow_Loaded;
            viewModel = blindViewModel;
        }

        private void MuteGroupToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //foreach (var type in Mutegroup.ChannelOrder)
            //{
            //    for
            //    ControlFactory.CreateBooleanUpDown(mutegroupPanel, channel.username, "mutes[")
            //}
        }
    }
}

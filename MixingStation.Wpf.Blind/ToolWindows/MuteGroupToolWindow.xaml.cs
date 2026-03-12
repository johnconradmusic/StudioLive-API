using MixingStation.Api;
using MixingStation.Api.Models;
using MixingStation.Api.ViewModels;
using MixingStation.Wpf.Blind.UserControls;
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

namespace MixingStation.Wpf.Blind.ToolWindows
{
    /// <summary>
    /// Interaction logic for MuteGroupToolWindow.xaml
    /// </summary>
    public partial class MuteGroupToolWindow : ToolWindow
    {

        List<bool> mutes { get; set; } = new();
        MixerRootViewModel viewModel;

        public MuteGroupToolWindow(MixerRootViewModel blindViewModel)
        {
            InitializeComponent();
            //Title = $"Mute Group {index} Assigns";
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

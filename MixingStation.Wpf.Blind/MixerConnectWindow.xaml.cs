using Microsoft.Extensions.DependencyInjection;
using MixingStation.Api.Services;
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

namespace MixingStation.Wpf.Blind
{
    /// <summary>
    /// Interaction logic for MixerConnectWindow.xaml
    /// </summary>
    public partial class MixerConnectWindow : Window
    {
        public MixerConnectWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // App.ServiceProvider.GetRequiredService<BroadcastService>().
        }
    }
}

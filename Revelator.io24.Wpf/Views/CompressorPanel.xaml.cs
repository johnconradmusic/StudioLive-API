using Presonus.StudioLive32.Api.Models;
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

namespace Presonus.StudioLive32.Wpf.Views
{
    /// <summary>
    /// Interaction logic for CompressorPanel.xaml
    /// </summary>
    public partial class CompressorPanel : Window
    {
        public CompressorPanel(ChannelBase channel)
        {
            InitializeComponent();
            DataContext = channel;
        }
    }
}

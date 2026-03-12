using Microsoft.Extensions.DependencyInjection;
using MixingStation.Api.Schema;
using MixingStation.Api.Services;
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

namespace MixingStation.Wpf.Blind
{
    /// <summary>
    /// Interaction logic for MixerConnectWindow.xaml
    /// </summary>
    public partial class MixerConnectWindow : Window
    {
        UiNodeBinder nodeBinder;
        public MixerConnectWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var service = App.ServiceProvider.GetRequiredService<MixingStationSessionService>();
            var stateService = App.ServiceProvider.GetRequiredService<MixerStateService>();
            service.ConnectAsync().Wait();
            var nodes = service.GetUINodes().Result;

            nodeBinder = new UiNodeBinder(stateService);
            nodeBinder.BindTree(nodes);

            WalkNodes(nodes.Children);
        }

        //need recursive node walker
        private void WalkNodes(IEnumerable<UiNode> nodes, int indent = 0)
        {
            try
            {
                foreach (var node in nodes)
                {
                    if (node.Path != null && node.Path.Contains("ch.0"))
                    {
                        switch (node.Kind)
                        {
                            case ParameterKind.Number:
                                var control = new NumericUpDown();
                                control.Min = (float)node.Min;
                                control.Max = (float)node.Max;
                                control.Unit = node.Unit;
                                control.Node = node;
                                MainGrid.Children.Add(control);
                                break;
                            case ParameterKind.Boolean:
                                var boolControl = new BooleanUpDown();
                                boolControl.Node = node;
                                MainGrid.Children.Add(boolControl);
                                break;
                        }
                    }
                    if (node.Children != null && node.Children.Any())
                    {
                        WalkNodes(node.Children, indent + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

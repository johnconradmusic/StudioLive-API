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

namespace Revelator.io24.Wpf
{
    /// <summary>
    /// Interaction logic for Mixer.xaml
    /// </summary>
    public partial class Mixer : Window
    {
        public Mixer(MainViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
#if DEBUG
            Environment.Exit(0);
#endif
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var res = vm?.Device.RawService.GetString("line/ch1/username");
            MessageBox.Show(res);
        }
    }
}

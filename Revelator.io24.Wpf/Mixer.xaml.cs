using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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
			vm.Device.RawService.JSON();
			ChannelList.Items.Refresh();

		}

        private void testButton_Click_1(object sender, RoutedEventArgs e)
        {
			var vm = DataContext as MainViewModel;
			vm.Device.RawService.SetValue("line/ch1/preampgain", .5f);
		}
    }
    public class MyList : ListView
	{
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			FrameworkElement source = element as FrameworkElement;

			source.SetBinding(AutomationProperties.AutomationIdProperty, new Binding
			{
				Path = new PropertyPath("Content.AutomationId"),
				RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self }
			});

			source.SetBinding(AutomationProperties.NameProperty, new Binding
			{
				Path = new PropertyPath("Content.AutomationName"),
				RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self }
			});
		}
	}
}

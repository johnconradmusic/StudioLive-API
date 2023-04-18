using Presonus.UCNet.Api;
using System.Windows;

namespace Presonus.UCNet.Wpf
{
	public partial class MainWindow : Window
	{
		public MainWindow(MainViewModel viewModel)
		{
			DataContext = viewModel;
			InitializeComponent();
		}
	}
}
using MahApps.Metro.Controls;
using Presonus.UCNet.Api;
using System.Windows;

namespace Presonus.UCNet.Wpf
{
	public partial class MainWindow : MetroWindow
	{
		public MainWindow(MainViewModel viewModel)
		{
			DataContext = viewModel;
			InitializeComponent();
		}
	}
}
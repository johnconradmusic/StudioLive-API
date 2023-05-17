using Presonus.UCNet.Api;
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
	/// Interaction logic for ChannelSelectorToolWindow.xaml
	/// </summary>
	public partial class ChannelSelectorToolWindow : ToolWindow
	{
		BlindViewModel _viewModel;
		private ListUpDown chanList;
		public int Selection { get; set; }
		public ChannelSelectorToolWindow(BlindViewModel blindViewModel)
		{
			_viewModel = blindViewModel;
			InitializeComponent();
			Title = $"Channel Finder";
			Loaded += ChannelSelectorToolWindow_Loaded;
			PreviewKeyDown += ChannelSelectorToolWindow_PreviewKeyDown;
		}

		private void ChannelSelectorToolWindow_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var selection = chanList.SelectedItem;
				foreach (var chan in _viewModel.AllChannels)
				{
					if (chan.username == selection)
					{
						//chan.select = true;
						DialogResult = true;
						Selection = _viewModel.AllChannels.IndexOf(chan);
						Close();
						return;
					}
				}
			}
		}

		private void ChannelSelectorToolWindow_Loaded(object sender, RoutedEventArgs e)
		{
			chanList = new ListUpDown();
			chanList.Caption = "Channels";
			chanList.Items = _viewModel.AllChannels.Select(c => c.username).ToList();

			routingPanel.Children.Add(chanList);

			chanList.Focus();
		}
	}
}

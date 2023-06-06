using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Wpf.Blind.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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
	/// Interaction logic for FileOpenToolWindow.xaml
	/// </summary>
	public partial class FileOpenToolWindow : ToolWindow
	{
		List<GenericListItem> _items;
		private ListUpDown listUpDown;

		public FileOpenToolWindow(List<GenericListItem> items, string title)
		{
			InitializeComponent();
			_items = items;
			Title = title;
			Loaded += FileOpenToolWindow_Loaded;
			PreviewKeyDown += FileOpenToolWindow_PreviewKeyDown;
		}

		private void FileOpenToolWindow_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				e.Handled = true;
				DialogResult = true;
				Selection = _items.Where(c => c.Title == listUpDown.SelectedItem).FirstOrDefault().Name;
				Close();
			}
		}

		public string Selection { get; set; }

		private void FileOpenToolWindow_Loaded(object sender, RoutedEventArgs e)
		{
			listUpDown = new ListUpDown();
			var titleList = _items.Select(c => c.Title).ToList();

			listUpDown.Items = titleList;

			routingPanel.Children.Add(listUpDown);
		}

	}
}

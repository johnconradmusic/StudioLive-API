using Presonus.UCNet.Api;
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
	/// Interaction logic for ProjectSceneFiltersToolWindow.xaml
	/// </summary>
	public partial class ProjectSceneFiltersToolWindow : ToolWindow
	{
		BlindViewModel _viewModel;
		public ProjectSceneFiltersToolWindow(BlindViewModel blindViewModel, string tag)
		{
			_viewModel = blindViewModel;
			DataContext = _viewModel;
			InitializeComponent();
			Title = $"Project and Scene Filters";
			scenePanel.Visibility = Visibility.Collapsed;
			projectPanel.Visibility = Visibility.Collapsed;
			switch (tag)
			{
				case "Project":
					projectPanel.Visibility = Visibility.Visible;
					break;
				case "Scene":
					scenePanel.Visibility = Visibility.Visible;
					break;
			}
		}
	}
}

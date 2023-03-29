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

namespace Presonus.UCNet.Api
{
	/// <summary>
	/// Interaction logic for MixerWindow.xaml
	/// </summary>
	public partial class MixerWindow : Window
	{
		public MixerWindow(MainViewModel viewModel)
		{
			InitializeComponent();
            DataContext = viewModel;
		}
	}
}

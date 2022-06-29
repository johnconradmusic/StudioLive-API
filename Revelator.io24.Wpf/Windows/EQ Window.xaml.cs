using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Presonus.StudioLive32.Wpf.Windows
{
	/// <summary>
	/// Interaction logic for EQ_Window.xaml
	/// </summary>
	public partial class EQ_Window : Window
	{
		public EQ_Window(ChannelBase channelBase)
		{
			InitializeComponent();			
		}
	}
}

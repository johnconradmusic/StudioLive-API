using Presonus.UCNet.Api.Models;
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

namespace Presonus.UCNet.Wpf.Blind.ToolWindows
{
	/// <summary>
	/// Interaction logic for TextPrompt.xaml
	/// </summary>
	public partial class TextPrompt : ToolWindow
	{
		PropertyInfo prop;
		Channel chan;
		public TextPrompt(Channel channel, PropertyInfo property, string msg) : base(channel)
		{
			chan = channel;
			prop = property;
			InitializeComponent();
			textBox.Text = (string)property.GetValue(channel);
			PreviewKeyDown += TextPrompt_PreviewKeyDown;
			Title = msg;
		}

		private void TextPrompt_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				prop.SetValue(chan, textBox.Text);
				Close();
			}

		}
	}
}

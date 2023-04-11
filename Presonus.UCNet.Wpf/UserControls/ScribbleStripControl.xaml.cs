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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presonus.UCNet.Wpf.UserControls
{
	public partial class ScribbleStripControl : UserControl
	{
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(ScribbleStripControl), new PropertyMetadata("Text"));

		public static readonly DependencyProperty StripColorProperty =
			DependencyProperty.Register("StripColor", typeof(SolidColorBrush), typeof(ScribbleStripControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

		private void ScribbleStripText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Rename();
		}
		bool editing;
		private void Rename()
		{
			TextBox textBox = new TextBox();
			textBox.Text = Text; // Set the initial text of the textbox to the current value of the Text property
			textBox.Focus();
			textBox.SelectAll();
			textBox.LostFocus += TextBox_LostFocus;
			textBox.KeyDown += TextBox_KeyDown;
			Grid parentGrid = (Grid)VisualTreeHelper.GetParent(ScribbleStripBackground); // Get the parent Grid of the Border
			parentGrid.Children.Add(textBox); // Add the TextBox to the parent Grid
			Grid.SetColumn(textBox, 0); // Set the column to 0 so it appears in the same column as the Border
			Grid.SetRow(textBox, 0); // Set the row to 0 so it appears in the same row as the Border

			ScribbleStripText.Visibility = Visibility.Collapsed; // Hide the TextBlock
			editing = true;
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter) // Check if the Enter key was pressed
			{
				TextBox textBox = (TextBox)sender;
				Text = textBox.Text; // Update the Text property with the new value
				textBox.LostFocus -= TextBox_LostFocus; // Remove the LostFocus event handler from the TextBox
				textBox.KeyDown -= TextBox_KeyDown; // Remove the KeyDown event handler from the TextBox
				Grid parentGrid = (Grid)VisualTreeHelper.GetParent(ScribbleStripBackground); // Get the parent Grid of the Border
				parentGrid.Children.Remove(textBox); // Add the TextBox to the parent Grid
				ScribbleStripText.Visibility = Visibility.Visible; // Show the TextBlock again
				editing = false;

			}
		}
		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			Text = textBox.Text; // Update the Text property with the new value
			textBox.LostFocus -= TextBox_LostFocus; // Remove the LostFocus event handler from the TextBox
			textBox.KeyDown -= TextBox_KeyDown; // Remove the KeyDown event handler from the TextBox
			Grid parentGrid = (Grid)VisualTreeHelper.GetParent(ScribbleStripBackground); // Get the parent Grid of the Border
			parentGrid.Children.Remove(textBox); // Add the TextBox to the parent Grid
			ScribbleStripText.Visibility = Visibility.Visible; // Show the TextBlock again
			editing = false;
		}
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public SolidColorBrush StripColor
		{
			get { return (SolidColorBrush)GetValue(StripColorProperty); }
			set { SetValue(StripColorProperty, value); }
		}

		public ScribbleStripControl()
		{
			InitializeComponent();
		}

		private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (!editing)
				if (e.Key == Key.Enter) 
					Rename();
		}
	}
}

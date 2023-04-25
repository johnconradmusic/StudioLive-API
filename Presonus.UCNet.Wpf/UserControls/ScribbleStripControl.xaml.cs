using Presonus.UCNet.Wpf.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Presonus.UCNet.Wpf.UserControls
{
	public partial class ScribbleStripControl : UserControl, IAccessibleControl
	{
		private bool editing;

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(ScribbleStripControl), new PropertyMetadata("", TextChanged));

		private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is Control control && control is IAccessibleControl accessibleControl)
			{
				accessibleControl.ValueString = (string)e.NewValue;
			}
		}

		public static readonly DependencyProperty StripColorProperty =
			DependencyProperty.Register("StripColor", typeof(SolidColorBrush), typeof(ScribbleStripControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));



		// Using a DependencyProperty as the backing store for ValueString. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty ValueStringProperty =
			DependencyProperty.Register("ValueString", typeof(string), typeof(ScribbleStripControl), new PropertyMetadata("unknown value"));

		// Using a DependencyProperty as the backing store for Caption. This enables animation,
		// styling, binding, etc...
		public static readonly DependencyProperty CaptionProperty =
			DependencyProperty.Register("Caption", typeof(string), typeof(ScribbleStripControl), new PropertyMetadata(""));

		public ScribbleStripControl()
		{
			InitializeComponent();
		}

		public event EventHandler ValueChanged;

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

		public string ValueString
		{
			get { return (string)GetValue(ValueStringProperty); }
			set { SetValue(ValueStringProperty, value); }
		}

		public string Caption
		{
			get { return (string)GetValue(CaptionProperty); }
			set { SetValue(CaptionProperty, value); }
		}

		private void ScribbleStripText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Rename();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ValueString = Text;
		}

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

		private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (!editing)
				if (e.Key == Key.Space)
					Rename();
		}
	}
}
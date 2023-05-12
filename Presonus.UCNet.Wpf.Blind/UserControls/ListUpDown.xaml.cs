using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Presonus.UCNet.Wpf.Blind.UserControls
{
	public class ListUpDownEventArgs : EventArgs
	{
		public ListUpDownEventArgs(float value, int index, string valueString)
		{
			Value = value;
			Index = index;
			ValueString = valueString;
		}

		public float Value { get; }
		public int Index { get; }
		public string ValueString { get; }
	}
	public partial class ListUpDown : UserControl
	{
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(float), typeof(ListUpDown),
				new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

		public static readonly DependencyProperty ValueStringProperty =
			DependencyProperty.Register("ValueString", typeof(string), typeof(ListUpDown), new PropertyMetadata("unknown value"));

		public static readonly DependencyProperty CaptionProperty =
			DependencyProperty.Register("Caption", typeof(string), typeof(ListUpDown), new PropertyMetadata(""));

		public static readonly DependencyProperty DefaultProperty =
		   DependencyProperty.Register("Default", typeof(float), typeof(ListUpDown), new PropertyMetadata(0f));

		public static readonly DependencyProperty ItemsProperty =
			DependencyProperty.Register("Items", typeof(List<string>), typeof(ListUpDown), new PropertyMetadata(null));

		public static readonly DependencyProperty SelectedIndexProperty =
			DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ListUpDown), new PropertyMetadata(0));

		public ListUpDown()
		{
			InitializeComponent();
			Loaded += ListUpDown_Loaded;
		}
		public delegate void ListUpDownValueChangedDelegate(object sender, ListUpDownEventArgs e);
		public event ListUpDownValueChangedDelegate ValueChanged;

		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}
		public string SelectedItem
		{
			get
			{
				if (Items == null || SelectedIndex < 0 || SelectedIndex >= Items.Count)
				{
					return null; // or throw an exception, depending on your requirements
				}
				return Items[SelectedIndex];
			}
		}
		public List<string> Items
		{
			get { return (List<string>)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}

		public float Default
		{
			get { return (float)GetValue(DefaultProperty); }
			set { SetValue(DefaultProperty, value); }
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

		public float Value
		{
			get { return (float)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ListUpDown;
			control.UpdateValueString();
			control.ValueChanged?.Invoke(control, new ListUpDownEventArgs(control.Value, control.SelectedIndex, control.ValueString));

		}

		private void ListUpDown_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateValueString();
		}

		private void UpdateValueString()
		{
			ValueString = GetItemFromFloat();
			if (IsFocused)
				Speech.SpeechManager.Say($"{ValueString}");
		}

		private void RotaryKnob_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				Speech.SpeechManager.Say($"{ValueString}");
			}
			if (e.Key == Key.Delete)
			{
				Value = Default;
			}
			if (e.Key == Key.Up || e.Key == Key.Down)
			{
				e.Handled = true;

				SelectedIndex += e.Key == Key.Up ? 1 : -1;
				SelectedIndex = Math.Max(Math.Min(SelectedIndex, Items.Count - 1), 0);

				Value = (float)SelectedIndex / (Items.Count - 1);
			}
		}

		private void UserControl_GotFocus(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			Speech.SpeechManager.Say($"{Caption} ({ValueString})");
		}

		public string GetItemFromFloat()
		{
			if (Items == null) return "no items";
			var _itemCount = Items.Count;

			if (Value > 1) Value = Value / Items.Count - 1;
			if (Value < 0) Value = 0;
			return Items[(int)Math.Round(Value * (_itemCount - 1))];
		}
	}
}
﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Presonus.StudioLive32.Wpf.UserControls
{
    public class StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? "" : string.Format("{0:N2}", value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class Fader : Control
    {
        // Using a DependencyProperty as the backing store for Fine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FineProperty =
            DependencyProperty.Register("Fine", typeof(bool), typeof(Fader), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(Fader), new PropertyMetadata("caption"));

        public static readonly RoutedEvent FocusedEvent
    = EventManager.RegisterRoutedEvent("Focused", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Fader));

        public static readonly RoutedEvent ValueChangedEvent
            = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Fader));

        public static readonly RoutedEvent TextChangedEvent
    = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Fader));

        public static readonly DependencyProperty DefaultProperty =
            DependencyProperty.Register("Default", typeof(double), typeof(Fader), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(Fader), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(Fader), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Fader), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty UnitProperty
            = DependencyProperty.Register(nameof(Unit), typeof(string), typeof(Fader), new PropertyMetadata(default(string)));

        public event RoutedEventHandler Focused
        {
            add { AddHandler(FocusedEvent, value); }
            remove { RemoveHandler(FocusedEvent, value); }
        }

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private enum ValueStepSize
        {
            Small = 1, Normal = 2, Large = 5
        }

        private double GetIncrement
        {
            get
            {
                if (Fine) return (Maximum - Minimum) / 10000;
                return (Maximum - Minimum) / 100;
            }
        }

        public bool Fine
        {
            get { return (bool)GetValue(FineProperty); }
            set { SetValue(FineProperty, value); }
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public double Default
        {
            get { return (double)GetValue(DefaultProperty); }
            set { SetValue(DefaultProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set
            {
                if (value > Maximum) value = Maximum;

                if (value < Minimum) value = Minimum;

                SetValue(ValueProperty, value);
                RoutedEventArgs args = new RoutedEventArgs(ValueChangedEvent);
                RaiseEvent(args);
            }
        }

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        private void Increment(ValueStepSize stepSize)
        {
            Value += GetIncrement * (int)stepSize;
        }

        private void Decrement(ValueStepSize stepSize)
        {
            Value -= GetIncrement * (int)stepSize;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            var textBox = Template.FindName("TextField", this) as TextBox;
            textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            var textBox = Template.FindName("TextField", this) as TextBox;
            textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var textBox = Template.FindName("TextField", this) as TextBox;
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                Increment(ValueStepSize.Normal);
                textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                Decrement(ValueStepSize.Normal);
                textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
            }
            if (e.Key == Key.PageUp)
            {
                e.Handled = true;
                Increment(ValueStepSize.Large);
                textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
            }
            if (e.Key == Key.PageDown)
            {
                e.Handled = true;
                Decrement(ValueStepSize.Large);
                textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (textBox != null)
                {
                    if (double.TryParse(textBox.Text, out double val))
                    {
                        Value = val;
                    }
                    textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
                }
            }
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                Value = Default;
                textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
            }
            //base.OnPreviewKeyDown(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var sign = Math.Sign(e.Delta);
            if (sign > 0) Value += GetIncrement / 2;
            else if (sign < 0) Value -= GetIncrement / 2;
            base.OnMouseWheel(e);
        }
    }
}
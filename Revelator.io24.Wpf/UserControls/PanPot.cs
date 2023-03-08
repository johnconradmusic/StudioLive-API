﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presonus.StudioLive32.Wpf.UserControls
{
    public class PanPot : Control
    {
        // Using a DependencyProperty as the backing store for LeftSideValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftSideValueProperty =
            DependencyProperty.Register("LeftSideValue", typeof(double), typeof(PanPot), new PropertyMetadata(default(double)));

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(PanPot), new PropertyMetadata(""));

        // Using a DependencyProperty as the backing store for RightSideValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightSideValueProperty =
            DependencyProperty.Register("RightSideValue", typeof(double), typeof(PanPot), new PropertyMetadata(default(double)));

        public static readonly RoutedEvent ValueChangedEvent
            = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PanPot));

        public static readonly RoutedEvent TextChangedEvent
    = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PanPot));

        // Using a DependencyProperty as the backing store for Default.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultProperty =
            DependencyProperty.Register("Default", typeof(double), typeof(PanPot), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(PanPot), new PropertyMetadata(default(double)));

        // Using a DependencyProperty as the backing store for .  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(PanPot), new PropertyMetadata(default(double)));

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(PanPot), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty UnitProperty
    = DependencyProperty.Register(nameof(Unit), typeof(string), typeof(PanPot), new PropertyMetadata(default(string)));

        public PanPot()
        {
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
                return (Maximum - Minimum) / 100;
            }
        }

        public double LeftSideValue
        {
            get { return (double)GetValue(LeftSideValueProperty); }
            private set { SetValue(LeftSideValueProperty, value); }
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public double RightSideValue
        {
            get { return (double)GetValue(RightSideValueProperty); }
            private set { SetValue(RightSideValueProperty, value); }
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
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                if (value > Maximum) value = Maximum;

                if (value < Minimum) value = Minimum;
                int val = Convert.ToInt32(value);
                if (val == 0)
                {
                    RightSideValue = 0;
                    LeftSideValue = 0;
                }
                if (val < 0)
                {
                    LeftSideValue = Math.Abs(val);
                    RightSideValue = 0;
                }
                if (val > 0)
                {
                    RightSideValue = Math.Abs(val);
                    LeftSideValue = 0;
                }
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                Increment(ValueStepSize.Normal);
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                Decrement(ValueStepSize.Normal);
            }
            if (e.Key == Key.PageUp)
            {
                e.Handled = true;
                Increment(ValueStepSize.Large);
            }
            if (e.Key == Key.PageDown)
            {
                e.Handled = true;
                Decrement(ValueStepSize.Large);
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                var textField = Template.FindName("TextField", this) as TextBox;
                if (textField != null)
                {
                    if (double.TryParse(textField.Text, out double val))
                    {
                        Value = val;
                    }
                }
            }
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                Value = Default;
            }
            //base.OnPreviewKeyDown(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var sign = Math.Sign(e.Delta);
            if (sign > 0) Value += GetIncrement;
            else if (sign < 0) Value -= GetIncrement;
            base.OnMouseWheel(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Value = Default;
            //var textField = Template.FindName("TextField", this) as TextBox;
            //textField.Text = "0";
        }
    }
}
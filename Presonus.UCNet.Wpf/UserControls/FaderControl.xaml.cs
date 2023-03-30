using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Services;
using Presonus.UCNet.Wpf.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presonus.StudioLive32.Wpf.UserControls
{
    public partial class FaderControl : UserControl, IAccessibleControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(float), typeof(FaderControl),
                new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(FaderControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(FaderControl),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(FaderControl), new PropertyMetadata("unknown control"));


        public string ValueString
        {
            get { return (string)GetValue(ValueStringProperty); }
            set { SetValue(ValueStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueStringProperty =
            DependencyProperty.Register("ValueString", typeof(string), typeof(FaderControl), new PropertyMetadata("unknown value"));



        public FaderControl()
        {
            InitializeComponent();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FaderControl;
            control?.UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            Console.WriteLine("Output: " + Math.Round(ValueTransformer.Transform(Value, 0.0001, 1, CurveFormula.LinearToVolume)));
        }

        public float Value
        {
            get => (float)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        private void UserControl_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Value += (float)e.Delta / 120f / 50f;
            Value = Math.Clamp(Value, 0f, 1f);
        }
    }

}
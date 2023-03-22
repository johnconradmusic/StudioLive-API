using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Presonus.StudioLive32.Wpf.UserControls
{
    public partial class RotaryKnobControl : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(RotaryKnobControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(RotaryKnobControl), new PropertyMetadata(50.0, OnSizeChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public RotaryKnobControl()
        {
            InitializeComponent();
            Knob.MouseLeftButtonDown += Knob_MouseLeftButtonDown;
            Knob.MouseMove += Knob_MouseMove;
            Knob.MouseLeftButtonUp += Knob_MouseLeftButtonUp;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ApplyControlSize();
            RotateIndicator(Value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (RotaryKnobControl)d;
            var newValue = (double)e.NewValue;
            control.RotateIndicator(newValue);
        }

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (RotaryKnobControl)d;
            control.ApplyControlSize();
        }

        private void ApplyControlSize()
        {
            LayoutRoot.Width = Size;
            LayoutRoot.Height = Size;
        }

        private void RotateIndicator(double value)
        {
            double angle = ConvertValueToAngle(value);
            var transform = new RotateTransform(angle, Indicator.ActualWidth / 2, Indicator.ActualHeight);
            Indicator.RenderTransform = transform;
        }

        private double ConvertValueToAngle(double value)
        {
            // Assuming a value range of 0 to 1 and angle range of -135 to 135
            return -135 + (value * 270);
        }

        private double ConvertAngleToValue(double angle)
        {
            return (angle + 135) / 270;
        }

        private bool _isDragging;
        private Point _previousMousePosition;

        private void Knob_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _previousMousePosition = e.GetPosition(this);
            _isDragging = true;
            Knob.CaptureMouse();
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                var currentPosition = e.GetPosition(this);
                var deltaY = currentPosition.Y - _previousMousePosition.Y;
                _previousMousePosition = currentPosition;

                // Assuming a value range of 0 to 1
                var newValue = Value - (deltaY / 270);
                newValue = Math.Max(0, Math.Min(1, newValue));

                Value = newValue;
            }
        }

        private void Knob_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            Knob.ReleaseMouseCapture();
		}
	}
}

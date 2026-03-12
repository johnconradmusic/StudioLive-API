using MixingStation.Api.Helpers;
using MixingStation.Api.Schema;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixingStation.Wpf.Blind.UserControls
{
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty NodeProperty =
            DependencyProperty.Register(
                nameof(Node),
                typeof(UiNode),
                typeof(NumericUpDown),
                new PropertyMetadata(null, OnNodeChanged));

        public static readonly DependencyProperty CurveProperty =
            DependencyProperty.Register(
                nameof(Curve),
                typeof(CurveFormula),
                typeof(NumericUpDown),
                new PropertyMetadata(CurveFormula.Linear));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(float),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(
                    0.0f,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register(
                nameof(Min),
                typeof(float),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(0f));

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register(
                nameof(Max),
                typeof(float),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(1f));

        public static readonly DependencyProperty MidProperty =
            DependencyProperty.Register(
                nameof(Mid),
                typeof(float),
                typeof(NumericUpDown),
                new PropertyMetadata(0f));

        public static readonly DependencyProperty ValueStringProperty =
            DependencyProperty.Register(
                nameof(ValueString),
                typeof(string),
                typeof(NumericUpDown),
                new PropertyMetadata("unknown value"));

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register(
                nameof(Caption),
                typeof(string),
                typeof(NumericUpDown),
                new PropertyMetadata(""));

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(
                nameof(Unit),
                typeof(string),
                typeof(NumericUpDown),
                new PropertyMetadata(""));

        public static readonly DependencyProperty DefaultProperty =
            DependencyProperty.Register(
                nameof(Default),
                typeof(float),
                typeof(NumericUpDown),
                new PropertyMetadata(0f));

        private bool _updatingFromNode;

        public NumericUpDown()
        {
            InitializeComponent();
            Loaded += NumericUpDown_Loaded;
        }

        public event EventHandler? ValueChanged;

        public UiNode? Node
        {
            get => (UiNode?)GetValue(NodeProperty);
            set => SetValue(NodeProperty, value);
        }

        public float Default
        {
            get => (float)GetValue(DefaultProperty);
            set => SetValue(DefaultProperty, value);
        }

        public CurveFormula Curve
        {
            get => (CurveFormula)GetValue(CurveProperty);
            set => SetValue(CurveProperty, value);
        }

        public float Mid
        {
            get => (float)GetValue(MidProperty);
            set => SetValue(MidProperty, value);
        }

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public float Min
        {
            get => (float)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        public float Max
        {
            get => (float)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        public float Value
        {
            get => (float)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public string ValueString
        {
            get => (string)GetValue(ValueStringProperty);
            set => SetValue(ValueStringProperty, value);
        }

        private static void OnNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)d;

            if (e.OldValue is UiNode oldNode)
                oldNode.PropertyChanged -= control.Node_PropertyChanged;

            if (e.NewValue is UiNode newNode)
            {
                newNode.PropertyChanged += control.Node_PropertyChanged;
                control.PullFromNode(newNode);
            }
            else
            {
                control.ClearFromNode();
            }
        }

        private void Node_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not UiNode node)
                return;

            if (e.PropertyName == nameof(UiNode.CurrentValue) ||
                e.PropertyName == nameof(UiNode.Label) ||
                e.PropertyName == nameof(UiNode.Min) ||
                e.PropertyName == nameof(UiNode.Max) ||
                e.PropertyName == nameof(UiNode.Step) ||
                e.PropertyName == nameof(UiNode.Unit))
            {
                PullFromNode(node);
            }
        }

        private void PullFromNode(UiNode node)
        {
            _updatingFromNode = true;
            try
            {
                Caption = node.Label;
                Unit = node.Unit ?? "";
                Min = (float)(node.Min ?? 0d);
                Max = (float)(node.Max ?? 1d);

                if (node.Step.HasValue && node.Min.HasValue && node.Max.HasValue)
                {
                    var mid = (node.Min.Value + node.Max.Value) / 2.0;
                    Mid = (float)mid;
                }

                Value = node.CurrentValue switch
                {
                    float f => f,
                    double d => (float)d,
                    int i => i,
                    _ => 0f
                };

                UpdateValueString();
            }
            finally
            {
                _updatingFromNode = false;
            }
        }

        private void ClearFromNode()
        {
            _updatingFromNode = true;
            try
            {
                Caption = "";
                Unit = "";
                Min = 0f;
                Max = 1f;
                Value = 0f;
                ValueString = "";
            }
            finally
            {
                _updatingFromNode = false;
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)d;
            control.UpdateValueString();

            if (!control._updatingFromNode && control.Node != null)
            {
                control.Node.CurrentValue = control.Value;
            }

            control.ValueChanged?.Invoke(control, EventArgs.Empty);
        }

        private void NumericUpDown_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateValueString();
        }

        private void UpdateValueString()
        {
            ValueString = string.IsNullOrWhiteSpace(Unit)
                ? $"{Value}"
                : $"{Value}{Unit}";

            if (IsFocused)
                Speech.SpeechManager.Say(ValueString);
        }

        private void RotaryKnob_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            float delta = 0f;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Speech.SpeechManager.Say(ValueString);
                return;
            }

            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                Value = Default;
                return;
            }

            if (e.Key == Key.Up)
            {
                e.Handled = true;
                delta = 1;
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                delta = -1;
            }
            else if (e.Key == Key.PageUp)
            {
                e.Handled = true;
                delta = 1 * 10f;
            }
            else if (e.Key == Key.PageDown)
            {
                e.Handled = true;
                delta = -1 * 10f;
            }

            if (ModifierKeys.IsCtrlDown())
                delta *= GetStep();

            if (delta != 0f)
            {
                float newValue = Math.Clamp(Value + delta, Min, Max);
                if (newValue != Value)
                    Value = newValue;
            }
        }

        private float GetStep()
        {
            if (Node?.Step is double step && step > 0)
                return (float)step;

            return 0.01f;
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Speech.SpeechManager.Say($"{Caption} ({ValueString})");
        }
    }
}
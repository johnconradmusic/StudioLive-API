using MixingStation.Api.Schema;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixingStation.Wpf.Blind.UserControls
{
    public partial class BooleanUpDown : UserControl
    {
        public static readonly DependencyProperty NodeProperty =
            DependencyProperty.Register(
                nameof(Node),
                typeof(UiNode),
                typeof(BooleanUpDown),
                new PropertyMetadata(null, OnNodeChanged));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(bool),
                typeof(BooleanUpDown),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        public static readonly DependencyProperty ValueStringProperty =
            DependencyProperty.Register(
                nameof(ValueString),
                typeof(string),
                typeof(BooleanUpDown),
                new PropertyMetadata("unknown value"));

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register(
                nameof(Caption),
                typeof(string),
                typeof(BooleanUpDown),
                new PropertyMetadata(""));

        public static readonly DependencyProperty DefaultProperty =
            DependencyProperty.Register(
                nameof(Default),
                typeof(bool),
                typeof(BooleanUpDown),
                new PropertyMetadata(false));

        private bool _updatingFromNode;

        public BooleanUpDown()
        {
            InitializeComponent();
            Loaded += BooleanUpDown_Loaded;
        }

        public event EventHandler? ValueChanged;

        public UiNode? Node
        {
            get => (UiNode?)GetValue(NodeProperty);
            set => SetValue(NodeProperty, value);
        }

        public bool Default
        {
            get => (bool)GetValue(DefaultProperty);
            set => SetValue(DefaultProperty, value);
        }

        public string ValueString
        {
            get => (string)GetValue(ValueStringProperty);
            set => SetValue(ValueStringProperty, value);
        }

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public bool Value
        {
            get => (bool)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BooleanUpDown)d;

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
                e.PropertyName == nameof(UiNode.Label))
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

                Value = node.CurrentValue switch
                {
                    bool b => b,
                    float f => f > 0.5f,
                    double d => d > 0.5,
                    int i => i != 0,
                    _ => false
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
                Caption = string.Empty;
                Value = false;
                ValueString = string.Empty;
            }
            finally
            {
                _updatingFromNode = false;
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BooleanUpDown)d;

            control.UpdateValueString();

            if (!control._updatingFromNode && control.Node != null)
            {
                control.Node.CurrentValue = control.Value;
            }

            control.ValueChanged?.Invoke(control, EventArgs.Empty);
        }

        private void BooleanUpDown_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateValueString();
        }

        private void UpdateValueString()
        {
            ValueString = Value ? "On" : "Off";

            if (IsFocused)
                Speech.SpeechManager.Say(ValueString);
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
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

            if (e.Key == Key.Down)
            {
                e.Handled = true;
                Value = false;
                return;
            }

            if (e.Key == Key.Up)
            {
                e.Handled = true;
                Value = true;
                return;
            }
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Speech.SpeechManager.Say($"{Caption} ({ValueString})");
        }
    }
}
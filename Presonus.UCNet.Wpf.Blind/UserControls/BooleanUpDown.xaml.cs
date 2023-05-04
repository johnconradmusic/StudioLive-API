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

namespace Presonus.UCNet.Wpf.Blind.UserControls
{
    /// <summary>
    /// Interaction logic for BooleanUpDown.xaml
    /// </summary>
    public partial class BooleanUpDown : UserControl
    {

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(bool), typeof(BooleanUpDown),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));


        // Using a DependencyProperty as the backing store for ValueString. This enables animation,
        // styling, binding, etc...
        public static readonly DependencyProperty ValueStringProperty =
            DependencyProperty.Register("ValueString", typeof(string), typeof(BooleanUpDown), new PropertyMetadata("unknown value"));

        // Using a DependencyProperty as the backing store for Caption. This enables animation,
        // styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(BooleanUpDown), new PropertyMetadata(""));



        // Using a DependencyProperty as the backing store for Default.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultProperty =
            DependencyProperty.Register("Default", typeof(bool), typeof(BooleanUpDown), new PropertyMetadata(false));


        public event EventHandler ValueChanged;

        public bool Default
        {
            get { return (bool)GetValue(DefaultProperty); }
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

        public bool Value
        {
            get { return (bool)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BooleanUpDown;
            control.UpdateValueString();
            control.ValueChanged?.Invoke(control, EventArgs.Empty);
        }

        private void NumericUpDown_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateValueString();
        }

        private void UpdateValueString()
        {
            ValueString = Value ? "On" : "Off";
            if (IsFocused)
                Speech.SpeechManager.Say($"{ValueString}");
        }
        public BooleanUpDown()
        {
            InitializeComponent();
            Loaded += BooleanUpDown_Loaded;
        }

        private void BooleanUpDown_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateValueString();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Speech.SpeechManager.Say(ValueString);
            }
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                Value = Default;
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                Value = false;
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                Value = true;
            }
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            Speech.SpeechManager.Say($"{Caption} ({ValueString})");
        }
    }
}

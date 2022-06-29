﻿using Presonus.StudioLive32.Api.Models;
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
using System.Windows.Shapes;

namespace Presonus.StudioLive32.Wpf.Views
{
    /// <summary>
    /// Interaction logic for AuxSendsView.xaml
    /// </summary>
    public partial class AuxSendsView : Window
    {
        public AuxSendsView(ChannelBase channel)
        {
            InitializeComponent();
            DataContext = channel;
        }
    }
}
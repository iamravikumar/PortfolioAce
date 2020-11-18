﻿using PortfolioAce.HelperObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PortfolioAce.Views.Modals
{
    /// <summary>
    /// Interaction logic for SecurityManagerWindow.xaml
    /// </summary>
    public partial class SecurityManagerWindow : Window
    {
        public SecurityManagerWindow()
        {
            InitializeComponent();
            cmbAssetClass.ItemsSource = StaticDataObjects.SecurityAssetClass;
            cmbCurrency.ItemsSource = StaticDataObjects.Currencies;
        }
    }
}
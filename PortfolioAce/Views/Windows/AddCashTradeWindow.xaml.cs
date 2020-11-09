using PortfolioAce.HelperObjects;
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

namespace PortfolioAce.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddCashTradeWindow.xaml
    /// </summary>
    public partial class AddCashTradeWindow : Window
    {
        public AddCashTradeWindow()
        {
            InitializeComponent();
            cmbCashType.ItemsSource = StaticDataObjects.CashTradeTypes;
            cmbCurrency.ItemsSource = StaticDataObjects.Currencies;
        }
    }
}

using PortfolioAce.HelperObjects;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for AddTradeWindow.xaml
    /// </summary>
    public partial class AddTradeWindow : Window
    {
        public AddTradeWindow()
        {
            InitializeComponent();
            cmbTradeType.ItemsSource = StaticDataObjects.SecurityTradeTypes;
            cmbCurrency.ItemsSource = StaticDataObjects.Currencies;
        }
    }
}

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

namespace PortfolioAce.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddFundWindow.xaml
    /// </summary>
    public partial class AddFundWindow : Window
    {
        public AddFundWindow()
        {
            InitializeComponent();
            List<string> currencies = Enum.GetNames(typeof(Currencies)).ToList();
            List<string> navfrequencies = Enum.GetNames(typeof(NavFrequency)).ToList();
            cmbNavFreq.ItemsSource = navfrequencies;
            cmbCurrency.ItemsSource = currencies;
        }
    }
}

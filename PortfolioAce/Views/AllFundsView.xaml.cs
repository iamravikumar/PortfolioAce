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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortfolioAce.Views
{
    /// <summary>
    /// Interaction logic for AllFundsView.xaml
    /// </summary>
    public partial class AllFundsView : UserControl
    {
        public AllFundsView()
        {
            InitializeComponent();
            List<DummyFundModel> funds = new List<DummyFundModel>();
            funds.Add(new DummyFundModel() { FundName = "Test Fund 1", FundSymbol = "TSTF1" });
            funds.Add(new DummyFundModel() { FundName = "Test Fund 2", FundSymbol = "TSTF2" });
            funds.Add(new DummyFundModel() { FundName = "Test Fund 3", FundSymbol = "TSTF3" });
            lbFundList.ItemsSource = funds;
        }
        
    }
    public class DummyFundModel
    {
        public string FundName { get; set; }
        public string FundSymbol { get; set; }
    }
}

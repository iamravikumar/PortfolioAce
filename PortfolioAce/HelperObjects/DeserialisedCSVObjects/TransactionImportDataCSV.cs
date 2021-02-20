using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects.DeserialisedCSVObjects
{
    public class TransactionImportDataCSV
    { 
        public string TransactionType { get; set; }
        public string SecuritySymbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime SettleDate { get; set; }
        public decimal Fees { get; set; }
        public string Currency { get; set; }
        public string Custodian { get; set; }
        public string FundSymbol { get; set; }
        public string Comment { get; set; }
    }
}

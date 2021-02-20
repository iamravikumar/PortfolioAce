using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects.DeserialisedCSVObjects
{
    public class TransactionImportDataCSV
    {
        [Name("Transaction Type")]
        public string TransactionType { get; set; }
        [Name("Security Symbol")]
        public string SecuritySymbol { get; set; }
        [Name("Quantity")]
        public decimal Quantity { get; set; }
        [Name("Price")]
        public decimal Price { get; set; }
        [Ignore()]
        public decimal TradeAmount { get { return Quantity*Price; } }
        [Name("Trade Date")]
        public DateTime TradeDate { get; set; }
        [Name("Settle Date")]
        public DateTime SettleDate { get; set; }
        [Name("Fees")]
        public decimal Fees { get; set; }
        [Name("Currency")]
        public string Currency { get; set; }
        [Name("Custodian")]
        public string Custodian { get; set; }
        [Name("Fund Symbol")]
        public string FundSymbol { get; set; }
        [Name("Comment")]
        public string Comment { get; set; }
        
    }
}

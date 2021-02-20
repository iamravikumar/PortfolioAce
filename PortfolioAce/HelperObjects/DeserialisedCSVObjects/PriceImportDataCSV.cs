using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects.DeserialisedCSVObjects
{
    public class PriceImportDataCSV
    {
        [Name("Security Symbol")]
        public string SecuritySymbol { get; set; }
        [Name("Date")]
        public DateTime Date { get; set; }
        [Name("Close Price")]
        public decimal ClosePrice { get; set; }
        [Ignore()]
        public string PriceSource { get { return "Manual"; } }
    }
}

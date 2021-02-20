using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects.DeserialisedCSVObjects
{
    public class PriceImportDataCSV
    {
        public string SecuritySymbol { get; set; }
        public DateTime Date { get; set; }
        public decimal ClosePrice { get; set; }
        public string PriceSource
        {
            get
            {
                return "Manual";
            }
        }

    }
}

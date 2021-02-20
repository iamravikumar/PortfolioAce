using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects.DeserialisedCSVObjects
{
    public class SecurityImportDataCSV
    {
        public string AssetClass { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Currency { get; set; }
        public string? ISIN { get; set; }
        public string AVSymbol { get; set; }
        public string FMPSymbol { get; set; }
    }
}

using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects.DeserialisedCSVObjects
{
    public class SecurityImportDataCSV
    {
        [Name("Asset Class")]
        public string AssetClass { get; set; }
        [Name("Name")]
        public string Name { get; set; }
        [Name("Symbol")]
        public string Symbol { get; set; }
        [Name("Currency")]
        public string Currency { get; set; }
        [Name("ISIN"), Optional]
        public string ISIN { get; set; }
        [Name("AV Symbol")]
        public string AVSymbol { get; set; }
        [Name("FMP Symbol")]
        public string FMPSymbol { get; set; }
    }

}

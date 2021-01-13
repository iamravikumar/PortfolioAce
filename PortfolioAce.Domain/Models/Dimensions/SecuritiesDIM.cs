using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    // maybe at some point create a security master that populates a skeleton
    // security with info from FMP...
    [Table("dim_Securities")]
    public class SecuritiesDIM
    {
        [Key]
        public int SecurityId { get; set; }
        [Required]
        public string Symbol { get; set; }

        // I should include a secondary symbol here specific for AlphaVantage API. This way i can have a custom Symbol above, and a seperate symbol for alphavantage this decouples the application from Alphavantage.
        /*
        It doesn't need to be required
        public string? AlphaVantageSymbol { get; set; }
         */
        [Required]
        public string SecurityName { get; set; }
        public string ISIN { get; set; }

        [ForeignKey("AssetClass")]
        public int AssetClassId { get; set; }
        public virtual AssetClassDIM AssetClass { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public virtual CurrenciesDIM Currency { get; set; }

    }
}

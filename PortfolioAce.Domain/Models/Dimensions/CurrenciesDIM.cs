using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_Currencies")]
    public class CurrenciesDIM
    {
        [Key]
        public int CurrencyId { get; set; }
        [Required]
        public ISOSymbol Symbol { get; set; } 
        [Required]
        public ISOName Name { get; set; } 
    }
    public enum ISOSymbol
    {
        GBP,
        EUR,
        USD,
        JPY,
        INR,
        CHF,
        CAD,
        AUD
    }
    public enum ISOName
    {
        PoundSterling,
        Euro,
        UnitedStatesDollar,
        JapaneseYen,
        IndianRupee,
        SwissFranc,
        CanadianDollar,
        AustralianDollar
    }
}

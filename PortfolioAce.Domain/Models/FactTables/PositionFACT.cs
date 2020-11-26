using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.FactTables
{
    [Table("fact_Positions")]
    public class PositionFACT
    {
        [Key]
        public int PositionId { get; set; }
        [Required]
        public decimal PositionDate { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal AverageCost { get; set; }
        [Required]
        public decimal MarketValue { get; set; }
        [Required]
        public decimal RealisedPnl { get; set; }
        [Required]
        public decimal UnrealisedPnl { get; set; }
        [Required]
        public decimal Price { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }

        [ForeignKey("AssetClass")]
        public int AssetClassId { get; set; }
        public virtual AssetClassDIM AssetClass { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public virtual CurrenciesDIM Currency { get; set; }

        [ForeignKey("Security")]
        public int SecurityId { get; set; }
        public virtual SecuritiesDIM Security { get; set; }
    }
}

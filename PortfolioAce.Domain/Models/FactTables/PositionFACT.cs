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
        // I need to a pseudo "position ID" unique for this position. so that i can link it to Trades... this will also allow me to calculate open lots..."
        // Trades will have a foreign key linking to this position
        [Key]
        public int PositionId { get; set; }
        [Required]
        public DateTime PositionDate { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal AverageCost { get; set; }
        [Required]
        public decimal MarketValue { get; set; } // This is base. I also need local..
        [Required]
        public decimal RealisedPnl { get; set; }
        [Required]
        public decimal UnrealisedPnl { get; set; } 
        [Required]
        public decimal Price { get; set; } // I think this should reference an actual security price..

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

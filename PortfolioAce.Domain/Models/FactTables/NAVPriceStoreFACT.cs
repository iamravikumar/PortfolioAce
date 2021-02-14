using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioAce.Domain.Models.FactTables
{
    [Table("fact_NAVPrices")]
    public class NAVPriceStoreFACT
    {
        [Key]
        public int NAVPriceId { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FinalisedDate { get; set; }

        [Required]
        public decimal NAVPrice { get; set; }

        [Required]
        public decimal SharesOutstanding { get; set; }

        [Required]
        public decimal NetAssetValue { get; set; }

        [Required]
        public string Currency { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }
        [ForeignKey("NAVPeriod")]
        public int NAVPeriodId { get; set; }
        public AccountingPeriodsDIM NAVPeriod { get; set; }
    }
}

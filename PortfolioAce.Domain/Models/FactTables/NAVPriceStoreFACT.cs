using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.FactTables
{
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
    }
}

using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.BackOfficeModels
{
    public class TradeBO
    {
        // dividends are accounted by setting the trade amount with 0 quantity and price
        [Key]
        public int TradeId { get; set; }

        [Required]
        public decimal Quantity { get; set; }
        [Required, Column(TypeName = "decimal(18,4)"), Range(0, long.MaxValue, ErrorMessage = "Prices can not be negative numbers.")]
        public decimal Price { get; set; }

        [Required]
        public decimal TradeAmount { get; set; } // this should be autocalculated in wpf when created and edit

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TradeDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SettleDate { get; set; }

        public decimal Commission { get; set; }

        public string TradeType { get; set; }

        public string Currency { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }

        [ForeignKey("Security")]
        public int SecurityId { get; set; }
        public virtual SecuritiesDIM Security { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    public class CashTrade
    {
        [Key]
        public int CashTradeId { get; set; }
        [Required]
        public string CashType { get; set; } // expense/income
        [Required]
        public decimal Amount { get; set; } // This has to be positive regardless of if its an expense or income.

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TradeDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SettleDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }
    }
}

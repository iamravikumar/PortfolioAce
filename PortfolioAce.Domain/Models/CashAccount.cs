using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    // This account should be uneditable directly.
    // Its crud operations are based on the CashTrade and Trade amounts.
    public class CashAccount
    {
        // This account is also known as the cash ledger
        [Key]
        public int TransactionId { get; set; }
        [Required]
        public string TransactionType { get; set; } // expense/income/Trade
        [Required]
        public decimal TransactionAmount { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string Currency { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }

        [ForeignKey("Trade")]
        public int? TradeId { get; set; }
        public Trade Trade { get; set; }

        [ForeignKey("CashTrade")]
        public int? CashTradeId { get; set; }
        public CashTrade CashTrade { get; set; }
    }
}

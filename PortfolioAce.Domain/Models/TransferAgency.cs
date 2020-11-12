using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    public class TransferAgency
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public DateTime TransactionDate { get; set; } // must be on the funds launch date!!!

        [Required]
        public DateTime TransactionSettleDate { get; set; } // this is the subscription and redemption dates.
        
        [Required]
        public string InvestorName { get; set; }
        
        [Required]
        public string Type { get; set; } //subscription and redemption date
        
        [Required]
        public decimal Units { get; set; }
        
        [Required]
        public decimal NAVPrice { get; set; }
        
        [Required]
        public decimal TradeAmount { get; set; }

        [Required]
        public string Currency { get; set; } // hardcoded to funds base currency... no support for share classes atm.

        [Required]
        public decimal Fees { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }
    }
}

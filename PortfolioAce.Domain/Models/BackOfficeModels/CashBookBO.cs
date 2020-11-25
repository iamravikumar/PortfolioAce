﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.BackOfficeModels
{
    // There should be no repo for this it is strictly a reference class. It is only updated via the CashTrades and Trades
    // Its crud operations are based on the CashTrade, InvestorActions and Trade amounts.

    public class CashBookBO
    {
        [Key]
        public int TransactionId { get; set; }
        [Required]
        public string TransactionType { get; set; } // expense/income/Trade/corp action/sub/red... this is the type as a string
        [Required]
        public decimal TransactionAmount { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }


        [ForeignKey("Trade")]
        public int? TradeId { get; set; }
        public TradeBO Trade { get; set; }

        [ForeignKey("CashTrade")]
        public int? CashTradeId { get; set; }
        public CashTradeBO CashTrade { get; set; }

        [ForeignKey("TransferAgent")]
        public int? TransferAgencyId { get; set; }
        public TransferAgencyBO TransferAgent { get; set; }
    }
}

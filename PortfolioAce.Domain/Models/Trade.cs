﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    public class Trade
    {
        // dividends are accounted by
        [Key]
        public int TradeId { get; set; }
        [Required]
        public string TradeType { get; set; } //trade corporate action
        [Required]
        public string Symbol { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required, Column(TypeName = "decimal(18,4)"), Range(0, long.MaxValue, ErrorMessage = "Prices can not be negative numbers.")]
        public decimal Price { get; set; }

        [Required]
        public decimal TradeAmount { get; set; }
        
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TradeDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SettleDate { get; set; }

        [Required]
        public string Currency { get; set; }
        public decimal Commission{ get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }
    }
}
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.FactTables
{
    [Table("fact_InvestorHoldings")]
    public class InvestorHoldingsFACT
    {
        [Key]
        public int HoldingId { get; set; }
        [Required]
        public decimal Units { get; set; }
        [Required]
        public decimal AverageCost { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime HoldingDate { get; set; }

        public decimal? HighWaterMark { get; set; }
        [Required]
        public decimal NetValuation { get; set; }
        [Required]
        public decimal ManagementFeesAccrued { get; set; }
        [Required]
        public decimal PerformanceFeesAccrued { get; set; }
        [ForeignKey("Investor")]
        public int InvestorId { get; set; }
        public InvestorsDIM Investor { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }

    }
}

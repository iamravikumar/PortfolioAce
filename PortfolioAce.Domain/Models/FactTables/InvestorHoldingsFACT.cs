using PortfolioAce.Domain.Models.BackOfficeModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.FactTables
{
    public class InvestorHoldingsFACT
    {
        [Key]
        public int HoldingId { get; set; }
        [Required]
        public decimal Units { get; set; }
        [Required]
        public decimal AverageCost { get; set; }
        [Required]
        public decimal HighWaterMark { get; set; }
        [Required]
        public decimal NetValuation { get; set; }
        [Required]
        public decimal ManagementFeesAccrued { get; set; }
        [Required]
        public decimal PerformanceFeesAccrued { get; set; }

    }
}

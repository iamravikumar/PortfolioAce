using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.BackOfficeModels
{
    public class InvestorDetails
    {
        // This class will contain the Highwatermark for now... BUT it will add flexibility if i decide to have different management and performance fees per investor..
        [Key]
        public int InvestorSettingId { get; set; }
        public decimal HighWaterMark { get; set; }
        [ForeignKey("Investor")]
        public int InvestorId { get; set; }
        public InvestorsBO Investor { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }
    }
}

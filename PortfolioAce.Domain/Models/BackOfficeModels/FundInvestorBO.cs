using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.BackOfficeModels
{
    public class FundInvestorBO
    {
        // This class will contain the Highwatermark for now... BUT it will add flexibility if i decide to have different management and performance fees per investor..
        // IMPORTANT NOTE: This is created when the fund is initialised OR if its the clients first trade on a fund...
        [Key]
        public int InvestorSettingId { get; set; }
        public decimal HighWaterMark { get; set; }
        [Required, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InceptionDate { get; set; }

        [ForeignKey("Investor")]
        public int InvestorId { get; set; }
        public InvestorsDIM Investor { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    public class Fund
    {
        [Key]
        public int FundId { get; set; }
        [Required]
        public string FundName { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string BaseCurrency { get; set; }
        [Required, Column(TypeName = "decimal(6,4)"), Range(0, 1, ErrorMessage = "Management Fee percentage is expressed with values between 0 and 1.")]
        public decimal ManagementFee { get; set; }
        [Required, Column(TypeName = "decimal(6,4)"), Range(0, 1, ErrorMessage = "Performance Fee percentage is expressed with values between 0 and 1.")]
        public decimal PerformanceFee { get; set; }
        [Required]
        public string NAVFrequency { get; set; }

        public virtual ICollection<Trade> Trades {get;set;}
        public virtual ICollection<CashAccount> CashAccounts { get; set; }

    }
}

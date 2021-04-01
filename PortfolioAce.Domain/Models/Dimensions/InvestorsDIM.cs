using PortfolioAce.Domain.Models.BackOfficeModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioAce.Domain.Models.Dimensions
{
    public class InvestorsDIM
    {
        [Key]
        public int InvestorId { get; set; }
        [Required, StringLength(50,MinimumLength =2)]
        public string FullName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BirthDate { get; set; }
        public string Domicile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string MobileNumber { get; set; }
        public string NativeLanguage { get; set; }

        public virtual ICollection<FundInvestorBO> FundInvestor { get; set; } // This means that i will need to create new investors first before i initialise a fund.


    }
}

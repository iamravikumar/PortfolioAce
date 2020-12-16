using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.BackOfficeModels
{
    public class InvestorsBO
    {
        [Key]
        public int InvestorId { get; set; }
        [Required]
        public string FullName { get; set; }

        //public decimal HighWaterMark { get; set; } // This means that i will have to link the investor to a fund but if i normalise this i won't...
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }
        public DateTime Domicile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string MobileNumber { get; set; }
        public string NativeLanguage { get; set; }

        public virtual ICollection<InvestorDetails> InvestorDetails { get; set; } // This means that i will need to create new investors first before i initialise a fund.


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    public class IssueTypesDIM
    {
        [Key]
        public int IssueTypeID { get; set; }
        [Required]
        public string TypeName { get; set; } // Subscription and Redemption
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    public class Security
    {
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Currency { get; set; }
        public string ISIN { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime IssueDate { get; set; }

    }
}

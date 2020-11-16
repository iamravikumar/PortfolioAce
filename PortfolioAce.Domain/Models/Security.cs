using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    public class Security
    {
        // Maybe add a sector class to this?
        [Key]
        public int SecurityId { get; set; }
        [Required]
        public string Symbol { get; set; }

        [Required]
        public string SecurityName { get; set; }

        [Required]
        public string Type { get; set; } // Rename this to AssetClass!!
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }
        public string ISIN { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime IssueDate { get; set; } // DELETE this not needed for now..

    }
}

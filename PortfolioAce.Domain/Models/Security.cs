using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    public class Security
    {
        [Key]
        public int SecurityId { get; set; }
        [Required]
        public string Symbol { get; set; }

        [Required]
        public string SecurityName { get; set; }

        [Required]
        public string AssetClass { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }
        public string ISIN { get; set; }

    }
}

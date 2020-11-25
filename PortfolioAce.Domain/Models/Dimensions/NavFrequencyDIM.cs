using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    public class NavFrequencyDIM
    {
        [Key]
        public int NavFrequencyId { get; set; }
        [Required]
        public string Frequency { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_NavFrequencies")]
    public class NavFrequencyDIM
    {
        [Key]
        public int NavFrequencyId { get; set; }
        [Required]
        public string Frequency { get; set; }
    }
}

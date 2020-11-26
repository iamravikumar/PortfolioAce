using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    // maybe at some point create a security master that populates a skeleton
    // security with info from FMP...
    [Table("dim_Securities")]
    public class SecuritiesDIM
    {
        [Key]
        public int SecurityId { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string SecurityName { get; set; }
        public string ISIN { get; set; }

        [Required]
        public string Currency { get; set; }
        [Required]
        public string AssetClass { get; set; }
    }
}

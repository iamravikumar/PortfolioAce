using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    public class CurrenciesDIM
    {
        [Key]
        public int CurrencyID { get; set; }
        [Required]
        public string Symbol { get; set; } // i.e. JPY
        [Required]
        public string Name { get; set; } // i.e. Japanese Yen
    }
}

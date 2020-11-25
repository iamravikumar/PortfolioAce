using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    public class TradeTypesDIM
    {
        [Key]
        public int SecurityTypeId { get; set; }
        [Required]
        public string TypeName { get; set; }
    }
}

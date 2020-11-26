using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_CashTradeTypes")]
    public class CashTradeTypesDIM
    {
        [Key]
        public int CashTypeId { get; set; }
        [Required]
        public string TypeName { get; set; } 
    }
}

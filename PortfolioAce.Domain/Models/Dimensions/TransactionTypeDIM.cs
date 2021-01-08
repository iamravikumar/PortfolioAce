using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_TransactionType")]
    public class TransactionTypeDIM
    {
        [Key]
        public int TransactionTypeId { get; set; }
        [Required]
        public string TypeName { get; set; } 
        [Required]
        public string TypeClass { get; set; } 
        [Required]
        public string Direction { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_Currencies")]
    public class CurrenciesDIM
    {
        [Key]
        public int CurrencyId { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_Custodians")]
    public class CustodiansDIM
    {
        [Key]
        public int CustodianId { get; set; }
        [Required]
        public string Symbol { get; set; } // i.e. IG, T212
        [Required]
        public string Name { get; set; } // i.e. IG Group, Trading 212
    }
}

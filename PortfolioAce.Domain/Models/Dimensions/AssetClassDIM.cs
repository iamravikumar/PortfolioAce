using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_AssetClasses")]
    public class AssetClassDIM
    {
        [Key]
        public int AssetClassId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

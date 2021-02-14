using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

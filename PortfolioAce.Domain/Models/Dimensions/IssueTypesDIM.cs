using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_IssueTypes")]
    public class IssueTypesDIM
    {
        [Key]
        public int IssueTypeID { get; set; }
        [Required]
        public string TypeName { get; set; } // Subscription and Redemption
    }
}

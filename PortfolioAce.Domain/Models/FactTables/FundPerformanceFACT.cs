using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioAce.Domain.Models.FactTables
{
    [Table("fact_FundPerformance")]
    public class FundPerformanceFACT
    {
        [Key]
        public int PerformanceID { get; set; }
    }
}

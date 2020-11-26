using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.FactTables
{
    [Table("fact_FundPerformance")]
    public class FundPerformanceFACT
    {
        [Key]
        public int PerformanceID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models.FactTables
{
    public class FundPerformanceFACT
    {
        [Key]
        public int PerformanceID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    // This is where lock periods are set.
    [Table("dim_Periods")]
    public class AccountingPeriodsDIM
    {
        [Key]
        public int PeriodId { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AccountingDate { get; set; }
        [Required]
        public bool isLocked { get; set; } // 1=locked period 0=unlocked
        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }
        // in the future maybe add a string that states the date it was locked on and what process locked it. i.e.
    }
}

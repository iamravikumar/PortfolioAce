using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    [Table("Fund")]
    public class Fund
    {
        [Key]
        public int FundId { get; set; }
        [Required]
        public string FundName { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string BaseCurrency { get; set; }
        [Required, Column(TypeName = "decimal(6,4)"), Range(0, 1, ErrorMessage = "Management Fee percentage is expressed with values between 0 and 1.")]
        public decimal ManagementFee { get; set; }
        [Required, Column(TypeName = "decimal(6,4)"), Range(0, 1, ErrorMessage = "Performance Fee percentage is expressed with values between 0 and 1.")]
        public decimal PerformanceFee { get; set; }
        [Required]
        public string NAVFrequency { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LaunchDate { get; set; }
        [Required]
        public bool IsInitialised { get; set; } // Defaults to False when Fund is first created, set to true once fund is initialised..

        public virtual ICollection<TransactionsBO> Transactions { get; set; }
        public virtual ICollection<TransferAgencyBO> TransferAgent {get;set;}
        public virtual ICollection<NAVPriceStoreFACT> NavPrices { get; set; }
        public virtual ICollection<AccountingPeriodsDIM> NavAccountingPeriods { get; set; }

    }
}

using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.BackOfficeModels
{
    // This will encompass Trades and CashTrade entries.  TransferAgency tasks will flow in as subs/reds (without the price and units)
    [Table("bo_Transactions")]
    public class TransactionsBO
    {
        [Key]
        public int TransactionId { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required, Column(TypeName = "decimal(18,4)"), Range(0, long.MaxValue, ErrorMessage = "Prices can not be negative numbers.")]
        public decimal Price { get; set; }
        [Required]
        public decimal TradeAmount { get; set; } // this should be autocalculated in wpf when created and edit. The sum of this will determine our cash

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TradeDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SettleDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; } // This is not set by the user

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LastModified { get; set; } // This is not set by the user

        public decimal Commission { get; set; } // Or fees
        [Required]
        public bool isActive { get; set; } // for audit purposes entries cannot be deleted, only deactivated
        [Required]
        public bool isLocked { get; set; } // if true then this trade cannot be edited or deleted unless the trade is unlocked

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [ForeignKey("Security")]
        public int SecurityId { get; set; }
        public SecuritiesDIM Security { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public CurrenciesDIM Currency { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }
        public Fund Fund { get; set; }

        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }
        public TransactionTypeDIM TransactionType { get; set; } //purchase sell income etc.. seperated by security or cash security or cash.. This will snowflake

        /*
         * Direction (purchase, sell, Income, Expense, Withdrawal, Deposit, dividends, Interest, management fee, perfromance fee, miscellaneous)
         * ^ These types will have another column that will be cash or security. This will allow me to differentiate on what should be where
         * Trade Type (Security or Cash) X
         * Quantity X
         * Price X
         * Transaction Amount X
         * Comments X
         * Trade Currency
         * trade date X
         * settle date X
         * created date X
         * last modified date X
         * Security I will have to securitise cash i.e. cash as a security will need to be seeded X
         * isLocked  X 
         * isActive X
         * Fees/Commission X
         *
         * subs and reds should be recorded here as such. but information such as price and quantity held in a seperate TA screen. effectively anything
         * that happens in the transfer agent screen will flow into this. think of an order management system or TA system. data from these sources
         * will directly feed into the back office once finalised..
         */

    }
}

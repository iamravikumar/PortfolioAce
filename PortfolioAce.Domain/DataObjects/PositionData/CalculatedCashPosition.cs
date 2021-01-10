using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public class CalculatedCashPosition
    {
        public CurrenciesDIM currency { get; }
        public CustodiansDIM custodian { get; }
        public DateTime? AsOfDate { get; set; }
        public decimal AccountBalance {get;set;}

        public CalculatedCashPosition(CurrenciesDIM currency, CustodiansDIM custodian)
        {
            this.currency = currency;
            this.custodian = custodian;
            this.AccountBalance = decimal.Zero;
            this.AsOfDate = DateTime.MinValue;
        }

        public void AddTransactions(List<TransactionsBO> transactions)
        {
            
            foreach(TransactionsBO transaction in transactions)
            {
                this.AddTransaction(transaction);
            }
        }

        public void AddTransaction(TransactionsBO transaction)
        {
            if (transaction.Currency.Symbol != this.currency.Symbol)
            {
                throw new InvalidOperationException("The transaction currency does not match the currency of this position");
            }

            if (transaction.Custodian.Name != this.custodian.Name)
            {
                throw new InvalidOperationException("These transactions belongs to a different custodian");
            }

            if(this.AsOfDate==null || this.AsOfDate < transaction.TradeDate)
            {
                this.AsOfDate = transaction.TradeDate;
            }
            this.AccountBalance += transaction.TradeAmount;
        }

    }
}

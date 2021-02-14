using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;

namespace PortfolioAce.Domain.DataObjects.PositionData
{

    public abstract class CalculatedCashPosition : CalculatedPosition
    {
        public abstract CurrenciesDIM Currency { get; }
        public abstract CustodiansDIM Custodian { get; }
        public abstract DateTime? AsOfDate { get; }
        public abstract decimal NetQuantity { get; }

        public abstract void AddTransaction(TransactionsBO transaction);
        public abstract void AddTransactionRange(List<TransactionsBO> transactions);

    }

    public class LiquidCashPosition : CalculatedCashPosition
    {
        private DateTime? _asOfDate;
        private decimal _balance;
        public override CurrenciesDIM Currency { get; }
        public override CustodiansDIM Custodian { get; }
        public override DateTime? AsOfDate { get { return _asOfDate; } }
        public override decimal NetQuantity { get { return _balance; } }

        public LiquidCashPosition(CurrenciesDIM currency, CustodiansDIM custodian)
        {
            this.Currency = currency;
            this.Custodian = custodian;
            this._balance = decimal.Zero;
            _asOfDate = DateTime.MinValue;
        }

        public override void AddTransactionRange(List<TransactionsBO> transactions)
        {

            foreach (TransactionsBO transaction in transactions)
            {
                this.AddTransaction(transaction);
            }
        }

        public override void AddTransaction(TransactionsBO transaction)
        {
            if (transaction.Currency.Symbol != this.Currency.Symbol)
            {
                throw new InvalidOperationException("The transaction currency does not match the currency of this position");
            }

            if (transaction.Custodian.Name != this.Custodian.Name)
            {
                throw new InvalidOperationException("These transactions belongs to a different custodian");
            }

            if (_asOfDate == null || _asOfDate < transaction.TradeDate)
            {
                _asOfDate = transaction.TradeDate;
            }
            _balance += transaction.TradeAmount;
        }
    }
}

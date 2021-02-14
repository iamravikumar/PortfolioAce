using System;
using System.Collections.Generic;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public abstract class ValuedCashPosition : ValuedPosition
    {
        public abstract CalculatedCashPosition CashPosition { get; }
        public abstract decimal MarketValueLocal { get; }
        public abstract decimal fxRate { get; }
        public abstract decimal MarketValueBase { get; }
        public abstract bool IsValuedBase { get; set; } // This determines whether fxRates and Market prices are available on this date...
        public abstract DateTime AsOfDate { get; set; }
    }

    public class ValuedLiquidCashPosition : ValuedCashPosition
    {
        public override CalculatedCashPosition CashPosition { get; }
        public override decimal MarketValueLocal { get { return CashPosition.NetQuantity; } }
        public override decimal fxRate { get { return _fxRate; } }
        public override decimal MarketValueBase { get { return MarketValueLocal / fxRate; } }
        public override bool IsValuedBase { get; set; } // This determines whether fxRates and Market prices are available on this date...
        public override DateTime AsOfDate { get; set; }
        private decimal _fxRate;
        public ValuedLiquidCashPosition(CalculatedCashPosition cashPosition, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            this.CashPosition = cashPosition;
            this.AsOfDate = asOfDate;

            string localCurrency = cashPosition.Currency.Symbol;
            if (localCurrency == FundBaseCurrency)
            {
                IsValuedBase = true;
                _fxRate = decimal.One;
            }
            else
            {
                string fxSymbol = $"{localCurrency}{FundBaseCurrency}";
                ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
                this.IsValuedBase = priceTable.ContainsKey(tableKeyFx);
                _fxRate = (this.IsValuedBase) ? priceTable[tableKeyFx] : decimal.One;
            }
        }
    }

}

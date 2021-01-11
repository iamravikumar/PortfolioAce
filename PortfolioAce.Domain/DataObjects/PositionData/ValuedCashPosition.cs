using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public abstract class ValuedCashPosition:ValuedPosition
    {
        public abstract CalculatedCashPosition CashPosition { get; set; }
        public abstract decimal MarketValueLocal { get; set; }
        public abstract decimal fxRate { get; set; }
        public abstract decimal MarketValueBase { get; set; }
        public abstract bool IsValuedBase { get; set; } // This determines whether fxRates and Market prices are available on this date...
        public abstract DateTime AsOfDate { get; set; }
    }
    
    public class ValuedLiquidCashPosition : ValuedCashPosition
    {
        public override CalculatedCashPosition CashPosition { get; set; }
        public override decimal MarketValueLocal { get; set; }
        public override decimal fxRate { get; set; }
        public override decimal MarketValueBase { get; set; }
        public override bool IsValuedBase { get; set; } // This determines whether fxRates and Market prices are available on this date...
        public override DateTime AsOfDate { get; set; }

        public ValuedLiquidCashPosition(CalculatedCashPosition cashPosition, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {

            this.AsOfDate = asOfDate;
            // this is for cash holdings I can get the valuation in base currency
            this.CashPosition = cashPosition;
            if (cashPosition.Currency.Symbol == FundBaseCurrency)
            {
                IsValuedBase = true;
                this.fxRate = decimal.One;
            }
            else
            {
                string fxSymbol = $"{cashPosition.Currency}{FundBaseCurrency}";
                ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
                this.IsValuedBase = priceTable.ContainsKey(tableKeyFx);
                this.fxRate = (priceTable.ContainsKey(tableKeyFx)) ? priceTable[tableKeyFx] : decimal.One;
            }
            this.MarketValueBase = Math.Round(this.CashPosition.NetQuantity * fxRate, 2);
        }
    }

}

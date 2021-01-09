using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public abstract class ValuedSecurityPosition
    {
        public abstract CalculatedSecurityPosition Position { get; set; }
        public abstract decimal MarketValueLocal { get; set; }
        public abstract decimal MarketValueBase { get;set; }
        public abstract decimal TotalPnL { get;set; }
        public abstract decimal unrealisedPnl { get;set; }
        public abstract decimal unrealisedPnLPercent { get;set; }
        public abstract decimal price { get; set; }
        public abstract decimal fxRate { get; set; }
        public abstract DateTime AsOfDate { get; set; }
        public abstract bool IsValuedBase { get; set; } // This determines whether fxRates and Market prices are available on this date...        
    }

    public class ValuedEquityPosition : ValuedSecurityPosition
    {
        public override CalculatedSecurityPosition Position { get; set; }

        public override decimal MarketValueLocal { get; set; }

        public override decimal MarketValueBase {get;set;}

        public override decimal TotalPnL {get;set;}

        public override decimal unrealisedPnl {get;set;}

        public override decimal unrealisedPnLPercent {get;set;}

        public override decimal price { get; set; }
        public override decimal fxRate {get;set;}
        public override DateTime AsOfDate {get;set;}
        public override bool IsValuedBase {get;set;}
        public ValuedEquityPosition(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            this.Position = position;
            this.AsOfDate = asOfDate;

            // these are temporary but i used this to make sure its valued..
            bool hasFxValue;
            bool hasSecurityValue;
            if (position.Security.Currency.Symbol == FundBaseCurrency)
            {
                hasFxValue = true;
                this.fxRate = decimal.One;
            }
            else
            {
                string fxSymbol = $"{position.Security.Currency}{FundBaseCurrency}";
                ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
                hasFxValue = priceTable.ContainsKey(tableKeyFx);
                this.fxRate = (priceTable.ContainsKey(tableKeyFx)) ? priceTable[tableKeyFx] : decimal.One;
            }

            ValueTuple<string, DateTime> tableKeySecurity = (position.Security.Symbol, asOfDate);
            hasSecurityValue = priceTable.ContainsKey(tableKeySecurity);
            this.price = priceTable.ContainsKey(tableKeySecurity) ? priceTable[tableKeySecurity] : decimal.Zero;

            this.IsValuedBase = (hasFxValue && hasSecurityValue);

            int multiplierPnL = (position.NetQuantity >= 0) ? 1 : -1;

            // raise exception if no fx rate is found.. (This might not be useful because USDUSD shouldnt be found...)
            this.MarketValueLocal = Math.Round(position.NetQuantity * price, 2);
            this.MarketValueBase = Math.Round(this.MarketValueLocal * fxRate, 2);
            this.unrealisedPnl = Math.Round(position.NetQuantity * (this.price - position.AverageCost) * multiplierPnL, 2);
            this.TotalPnL = this.unrealisedPnl + position.RealisedPnL;
        }

    }


    public class ValuedCryptoPosition : ValuedSecurityPosition
    {
        public override CalculatedSecurityPosition Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal MarketValueLocal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal MarketValueBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal TotalPnL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal unrealisedPnl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal unrealisedPnLPercent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal price { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal fxRate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override DateTime AsOfDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsValuedBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
    public class ValuedFXPosition : ValuedSecurityPosition
    {
        public override CalculatedSecurityPosition Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal MarketValueLocal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal MarketValueBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal TotalPnL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal unrealisedPnl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal unrealisedPnLPercent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal price { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override decimal fxRate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override DateTime AsOfDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsValuedBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}

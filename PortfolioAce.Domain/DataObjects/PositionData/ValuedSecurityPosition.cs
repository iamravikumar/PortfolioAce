using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public abstract class ValuedSecurityPosition:ValuedPosition
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
        public override CalculatedSecurityPosition Position { get; set; }

        public override decimal MarketValueLocal { get; set; }

        public override decimal MarketValueBase { get; set; }

        public override decimal TotalPnL { get; set; }

        public override decimal unrealisedPnl { get; set; }

        public override decimal unrealisedPnLPercent { get; set; }

        public override decimal price { get; set; }
        public override decimal fxRate { get; set; }
        public override DateTime AsOfDate { get; set; }
        public override bool IsValuedBase { get; set; }
        public ValuedCryptoPosition(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
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
    
    public class ValuedFXPosition : ValuedSecurityPosition
    {
        public override CalculatedSecurityPosition Position { get; set; }

        public override decimal MarketValueLocal { get; set; }

        public override decimal MarketValueBase { get; set; }

        public override decimal TotalPnL { get; set; }

        public override decimal unrealisedPnl { get; set; }

        public override decimal unrealisedPnLPercent { get; set; }
        private string UnderlyingCurrencyPair
        {
            get
            {
                return Position.Security.Symbol.Substring(0, 6);
            }
        }
        private string UnderlyingBaseCurrency
        {
            get
            {
                return Position.Security.Symbol.Substring(0, 3);
            }
        }
        private string UnderlyingQuoteCurrency
        {
            get
            {
                return Position.Security.Symbol.Substring(3, 3);
            }
        }

        private DateTime Maturity
        {
            get
            {
                DateTime maturity = DateTime.ParseExact(Position.Security.Symbol.Substring(6, 6),"ddMMyy", CultureInfo.InvariantCulture);
                return maturity;
            }
        }

        private decimal DeliveryPrice
        {
            get
            {
                return Position.AverageCost;
            }
        }

        public override decimal price { get; set; }
        public override decimal fxRate { get; set; }
        public override DateTime AsOfDate { get; set; }
        public override bool IsValuedBase { get; set; }
        public ValuedFXPosition(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            this.Position = position;
            this.AsOfDate = asOfDate;
            // Refactor this all to use fields instead so its clearer


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

            ValueTuple<string, DateTime> tableKeySecurity = (this.UnderlyingCurrencyPair, asOfDate); // This checks to see if i have the current spot rate...

            
            ValueTuple<string, DateTime> tableKeyBaseIR = ($"{this.UnderlyingBaseCurrency}_IRBASE", asOfDate); // This checks to see if i have the base currencys interest rate
            ValueTuple<string, DateTime> tableKeyQuoteIR = ($"{this.UnderlyingQuoteCurrency}_IRBASE", asOfDate); // This checks to see if i have the quote currencys interest rate
            // GBP/USD,  buy GBP sell USD (in setup ccy is GBP)
            // at T we pay USD, we recieve GBP


            decimal yearsToMaturity = ( this.Maturity.DayOfYear- asOfDate.DayOfYear) /360m; // I assume ACT/360 daycount.

            decimal currentSpot = priceTable[tableKeySecurity];
            decimal baseIR = priceTable[tableKeyBaseIR];
            decimal quoteIR = priceTable[tableKeyQuoteIR];

            double basePV = (double)Position.NetQuantity * Math.Exp((double)(-baseIR*yearsToMaturity));  //rec
            double quotePV =(double)-(Position.NetQuantity/Position.AverageCost) * Math.Exp((double)(-quoteIR*yearsToMaturity))*(double)currentSpot; // pay
            decimal MV =(decimal)(basePV + quotePV); // THIS IS THE MARKET VALUE OF MY FORWARD!!!! I COULD USE THE OTHER FORMULA TO GET THE FORWARD RATE SO I CAN CACULATE UNREALISED PNL...
            
            this.price = priceTable.ContainsKey(tableKeySecurity) ? priceTable[tableKeySecurity] : decimal.Zero;
            hasSecurityValue = priceTable.ContainsKey(tableKeySecurity) && priceTable.ContainsKey(tableKeyBaseIR) && priceTable.ContainsKey(tableKeyQuoteIR);


            this.IsValuedBase = (hasFxValue && hasSecurityValue);

            int multiplierPnL = (position.NetQuantity >= 0) ? 1 : -1;

            // raise exception if no fx rate is found.. (This might not be useful because USDUSD shouldnt be found...)
            this.MarketValueLocal = Math.Round(position.NetQuantity * price, 2);
            this.MarketValueBase = Math.Round(this.MarketValueLocal * fxRate, 2);
            this.unrealisedPnl = Math.Round(position.NetQuantity * (this.price - position.AverageCost) * multiplierPnL, 2);
            this.TotalPnL = this.unrealisedPnl + position.RealisedPnL;
        }
    }
}

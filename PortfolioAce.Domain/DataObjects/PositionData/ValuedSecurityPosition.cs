using System;
using System.Collections.Generic;
using System.Globalization;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public abstract class ValuedSecurityPosition : ValuedPosition
    {
        public abstract CalculatedSecurityPosition Position { get; }
        public abstract decimal MarketValueLocal { get; }
        public abstract decimal MarketValueBase { get; }
        public abstract decimal TotalPnL { get; }
        public abstract decimal UnrealisedPnl { get; }
        public abstract decimal UnrealisedPnLPercent { get; set; }
        public abstract decimal MarketPrice { get; }
        public abstract decimal fxRate { get; }
        public abstract DateTime AsOfDate { get; set; }
        public abstract bool IsValuedBase { get; set; } // This determines whether fxRates and Market prices are available on this date...        
    }

    public class ValuedEquityPosition : ValuedSecurityPosition
    {
        public override CalculatedSecurityPosition Position { get; }

        public override decimal MarketValueLocal { get { return _marketPrice * Position.NetQuantity; } }

        public override decimal MarketValueBase { get { return (_marketPrice * Position.NetQuantity) / _fxRate; } }

        public override decimal TotalPnL { get { return UnrealisedPnl + Position.RealisedPnL; } }

        public override decimal UnrealisedPnl
        {
            get
            {
                int multiplier = (Position.NetQuantity >= 0) ? 1 : -1;
                return Position.NetQuantity * (_marketPrice - Position.AverageCost) * multiplier;
            }
        }

        public override decimal UnrealisedPnLPercent { get; set; }

        public override decimal MarketPrice { get { return _marketPrice; } }
        public override decimal fxRate { get { return _fxRate; } }
        public override DateTime AsOfDate { get; set; }
        public override bool IsValuedBase { get; set; }
        private decimal _marketPrice;
        private decimal _fxRate;

        public ValuedEquityPosition(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            this.Position = position;
            this.AsOfDate = asOfDate;
            string localCurrency = position.Security.Currency.Symbol;
            bool hasFxValue;
            bool hasSecurityValue;

            if (localCurrency == FundBaseCurrency)
            {
                hasFxValue = true;
                _fxRate = decimal.One;
            }
            else
            {
                string fxSymbol = $"{localCurrency}{FundBaseCurrency}";
                ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
                hasFxValue = priceTable.ContainsKey(tableKeyFx);
                _fxRate = (hasFxValue) ? priceTable[tableKeyFx] : decimal.One;
            }

            ValueTuple<string, DateTime> tableKeySecurity = (position.Security.Symbol, asOfDate);
            hasSecurityValue = priceTable.ContainsKey(tableKeySecurity);
            _marketPrice = (hasSecurityValue) ? priceTable[tableKeySecurity] : decimal.Zero;

            this.IsValuedBase = (hasFxValue && hasSecurityValue);
        }

    }


    public class ValuedCryptoPosition : ValuedSecurityPosition
    {
        public override CalculatedSecurityPosition Position { get; }

        public override decimal MarketValueLocal { get { return _marketPrice * Position.NetQuantity; } }

        public override decimal MarketValueBase { get { return (_marketPrice * Position.NetQuantity) / _fxRate; } }

        public override decimal TotalPnL { get { return UnrealisedPnl + Position.RealisedPnL; } }

        public override decimal UnrealisedPnl
        {
            get
            {
                int multiplier = (Position.NetQuantity >= 0) ? 1 : -1;
                return Position.NetQuantity * (_marketPrice - Position.AverageCost) * multiplier;
            }
        }

        public override decimal UnrealisedPnLPercent { get; set; }

        public override decimal MarketPrice { get { return _marketPrice; } }
        public override decimal fxRate { get { return _fxRate; } }
        public override DateTime AsOfDate { get; set; }
        public override bool IsValuedBase { get; set; }
        private decimal _marketPrice;
        private decimal _fxRate;

        public ValuedCryptoPosition(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            this.Position = position;
            this.AsOfDate = asOfDate;

            string localCurrency = position.Security.Currency.Symbol;
            bool hasFxValue;
            bool hasSecurityValue;

            if (localCurrency == FundBaseCurrency)
            {
                hasFxValue = true;
                _fxRate = decimal.One;
            }
            else
            {
                string fxSymbol = $"{localCurrency}{FundBaseCurrency}";
                ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
                hasFxValue = priceTable.ContainsKey(tableKeyFx);
                _fxRate = (hasFxValue) ? priceTable[tableKeyFx] : decimal.One;
            }

            ValueTuple<string, DateTime> tableKeySecurity = (position.Security.Symbol, asOfDate);
            hasSecurityValue = priceTable.ContainsKey(tableKeySecurity);
            _marketPrice = (hasSecurityValue) ? priceTable[tableKeySecurity] : decimal.Zero;

            this.IsValuedBase = (hasFxValue && hasSecurityValue);
        }
    }

    public class ValuedFXForwardPosition : ValuedSecurityPosition
    {
        public override CalculatedSecurityPosition Position { get; }

        public override decimal MarketValueLocal
        {
            get
            {
                if (IsValuedBase)
                {
                    double basePV = (double)Position.NetQuantity * Math.Exp((double)(-_baseInterestRate * _yearsToMaturity));  //rec
                    double quotePV = (double)-(Position.NetQuantity / _deliveryPrice) * Math.Exp((double)(-_quoteInterestRate * _yearsToMaturity)) * (double)_currentSpot; // pay
                    return (decimal)(basePV + quotePV);
                }
                else
                {
                    return decimal.Zero;
                }
            }
        }

        public override decimal MarketValueBase
        {
            get
            {
                if (IsValuedBase)
                {
                    double basePV = (double)Position.NetQuantity * Math.Exp((double)(-_baseInterestRate * _yearsToMaturity));  //rec
                    double quotePV = (double)-(Position.NetQuantity / _deliveryPrice) * Math.Exp((double)(-_quoteInterestRate * _yearsToMaturity)) * (double)_currentSpot; // pay
                    return (decimal)(basePV + quotePV) / _fxRate;
                }
                else
                {
                    return decimal.Zero;
                }
            }
        }

        public override decimal TotalPnL
        {
            get
            {
                return UnrealisedPnl + Position.RealisedPnL;
            }
        }

        public override decimal UnrealisedPnl
        {
            get
            {
                // unrealised gain/loss is measured by applying todays market rates at forward date

                return MarketValueLocal;

            }
        }

        public override decimal UnrealisedPnLPercent { get; set; }


        public override decimal MarketPrice { get { return _currentSpot; } }
        public override decimal fxRate { get { return _fxRate; } }
        public override DateTime AsOfDate { get; set; }
        public override bool IsValuedBase { get; set; }

        private decimal ForwardRate
        {
            get
            {
                decimal irp = (_baseInterestRate - _quoteInterestRate) * _yearsToMaturity;
                double rate = (double)_currentSpot * Math.Exp((double)irp);
                return (decimal)rate;
            }
        }

        private decimal _currentSpot;
        private decimal _fxRate;
        private decimal _deliveryPrice;
        private decimal _baseInterestRate;
        private decimal _quoteInterestRate;
        private decimal _yearsToMaturity;
        private readonly string _underlyingCurrencyPair;
        private readonly string _underlyingBaseCurrency;
        private readonly string _underlyingQuoteCurrency;
        private readonly DateTime _maturityDate;


        public ValuedFXForwardPosition(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            this.Position = position;
            this.AsOfDate = asOfDate;

            _maturityDate = DateTime.ParseExact(Position.Security.Symbol.Substring(6, 6), "ddMMyy", CultureInfo.InvariantCulture);
            _yearsToMaturity = (_maturityDate.DayOfYear - asOfDate.DayOfYear) / 360m; // I assume ACT/360 daycount.
            _deliveryPrice = Position.AverageCost;


            _underlyingCurrencyPair = Position.Security.Symbol.Substring(0, 6);
            _underlyingBaseCurrency = _underlyingCurrencyPair.Substring(0, 3);
            _underlyingQuoteCurrency = _underlyingCurrencyPair.Substring(3, 3);

            bool hasFxValue;
            bool hasSecurityValue;
            if (position.Security.Currency.Symbol == FundBaseCurrency)
            {
                hasFxValue = true;
                _fxRate = decimal.One;
            }
            else
            {
                string fxSymbol = $"{position.Security.Currency.Symbol}{FundBaseCurrency}";
                ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
                hasFxValue = priceTable.ContainsKey(tableKeyFx);
                _fxRate = (priceTable.ContainsKey(tableKeyFx)) ? priceTable[tableKeyFx] : decimal.One;
            }

            ValueTuple<string, DateTime> tableKeySecurity = (_underlyingCurrencyPair, asOfDate); // This checks to see if i have the current spot rate...
            ValueTuple<string, DateTime> tableKeyBaseIR = ($"{_underlyingBaseCurrency}_IRBASE", asOfDate); // This checks to see if i have the base currencys interest rate
            ValueTuple<string, DateTime> tableKeyQuoteIR = ($"{_underlyingQuoteCurrency}_IRBASE", asOfDate); // This checks to see if i have the quote currencys interest rate
            // GBP/USD,  buy GBP sell USD (in setup ccy is GBP)
            // at T we pay USD, we recieve GBP
            hasSecurityValue = priceTable.ContainsKey(tableKeySecurity) && priceTable.ContainsKey(tableKeyBaseIR) && priceTable.ContainsKey(tableKeyQuoteIR); // This means we can value the Forward

            _currentSpot = priceTable.ContainsKey(tableKeySecurity) ? priceTable[tableKeySecurity] : decimal.Zero;
            _baseInterestRate = priceTable.ContainsKey(tableKeyBaseIR) ? priceTable[tableKeyBaseIR] : decimal.Zero;
            _quoteInterestRate = priceTable.ContainsKey(tableKeyQuoteIR) ? priceTable[tableKeyQuoteIR] : decimal.Zero;

            this.IsValuedBase = (hasFxValue && hasSecurityValue);


        }
    }
}

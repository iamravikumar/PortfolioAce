using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Domain.DataObjects
{
    public class NavValuations
    {
        // I need the fund to calculate performance and management fees... I can also reuse it to get the base currencies...
        public NavValuations(List<CalculatedSecurityPosition> securityPositions, List<CalculatedCashPosition> cashPositions, 
            Dictionary<(int, DateTime), decimal> priceTable, DateTime asOfDate, Fund fund)
        {

        }
    }

    public class SecurityPositionValuation
    {
        // This is perfect but i need to use a strategy pattern to implement it..
        // see https://stackoverflow.com/a/21369859/12148778
        public CalculatedSecurityPosition Position { get; set; }
        public decimal MarketValueLocal { get; set; }
        public decimal MarketValueBase { get; set; }
        public decimal unrealisedPnl { get; set; }
        public decimal unrealisedPnLPercent { get; set; }
        public decimal price { get; set; }
        public decimal fxRate { get; set; }
        public DateTime AsOfDate { get; set; }

        //public decimal TotalPnl {get;} this is unrealised *base
        public SecurityPositionValuation(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            this.Position = position;
            this.AsOfDate = asOfDate;
            string fxSymbol = $"{position.security.Currency}{FundBaseCurrency}";
            ValueTuple<string, DateTime> tableKeySecurity = (position.security.Symbol, asOfDate);
            ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
            int multiplierPnL = (position.NetQuantity >= 0) ? 1 : -1;

            // raise exception if no fx rate is found.. (This might not be useful because USDUSD shouldnt be found...)
            this.price = priceTable.ContainsKey(tableKeySecurity) ? priceTable[tableKeySecurity] : decimal.Zero; 
            this.fxRate = priceTable.ContainsKey(tableKeyFx) ? priceTable[tableKeyFx] : decimal.One; // i can then compare this against values to get base FX rate..
            this.MarketValueLocal = Math.Round(position.NetQuantity * price, 2);
            this.MarketValueBase = Math.Round(this.MarketValueLocal * fxRate, 2);
            this.unrealisedPnl = Math.Round(position.NetQuantity * (this.price - position.AverageCost) * multiplierPnL, 2);
        }
    }
    public class CashPositionValuation
    {
        public CashPositionValuation()
        {
            // this is for cash holdings I can get the valuation in base currency
        }
    }
    
}

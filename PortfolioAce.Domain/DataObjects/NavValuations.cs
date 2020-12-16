using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.Domain.DataObjects
{
    public class NavValuations
    {
        // I need the fund model to management fees, NAV and NAVps... I can also reuse it to get the base currencies...
        // Performance fees aren't calculated here... They are done an investor level since investors can buy/sell at different prices...
        // effectively i need a Holdings FACT Table for transfer agent
        public Fund fund { get; set; }
        public decimal ManagementFeeAmount { get; set; }
        public List<SecurityPositionValuation> SecurityPositions { get; set; } // when i put this in a datagrid i can check what is fullyvalued from what isn't
        public List<CashPositionValuation> CashPositions { get; set; } // when i put this in a datagrid i can check what is fullyvalued from what isn't
        public List<ClientHoldingValuation> ClientHoldings { get; set; }
        public DateTime AsOfDate { get; set; }
        public decimal NetAssetValue { get; set; }
        public decimal GrossAssetValue { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal NetAssetValuePerShare { get; set; }
        public decimal GrossAssetValuePerShare { get; set; }
        public int UnvaluedPositions { get; set; } // number of positions that do not have a price...
        public NavValuations(List<SecurityPositionValuation> securityPositions, List<CashPositionValuation> cashPositions, List<ClientHolding> clientHoldings,
            DateTime asOfDate, Fund fund)
        {
            this.SecurityPositions = securityPositions;
            this.CashPositions = cashPositions;
            this.fund = fund;
            this.AsOfDate = asOfDate;
            this.ClientHoldings = new List<ClientHoldingValuation>();
            this.Initialisation(clientHoldings);// this clientHoldings parameter is temporary. When i make the NavValuations live in the view i will refactor this..
        }

        private void Initialisation(List<ClientHolding> clientHoldings)
        {
            // This will initialise the calculations
            decimal securityNav = SecurityPositions.Sum(sp => sp.MarketValueBase);
            decimal cashNav = CashPositions.Sum(cp => cp.MarketValueBase);
            this.UnvaluedPositions = SecurityPositions.Where(sp => !sp.IsValuedBase).Count() + CashPositions.Where(cp => !cp.IsValuedBase).Count();
            this.GrossAssetValue = securityNav + cashNav;
            this.SharesOutstanding = this.fund.TransferAgent.Sum(ta => ta.Units); // if i make the units absolute i will have to negative units..
            this.GrossAssetValuePerShare = Math.Round(this.GrossAssetValue / this.SharesOutstanding, 5);

            //accruals
            int accrualPeriods;
            if(fund.NAVFrequency== "Daily")
            {
                accrualPeriods = (!DateTime.IsLeapYear(AsOfDate.Year) ? 365 : 366);
            }
            else
            {
                accrualPeriods = 12;
            }
            // REMEMBER for performance i need to calculate the holding period return
            // HPR = (Ending Value) / (Previous Value After Cash Flow) – 1.
            if (AsOfDate.DayOfWeek == DayOfWeek.Monday)
            {
                this.ManagementFeeAmount = 3*((GrossAssetValue * fund.ManagementFee) / accrualPeriods); // this will then be weighted on the investors..
            }
            else
            {
                this.ManagementFeeAmount = (GrossAssetValue * fund.ManagementFee) / accrualPeriods; // this will then be weighted on the investors..
            }
            this.NetAssetValue = GrossAssetValue - this.ManagementFeeAmount;
            this.NetAssetValuePerShare = Math.Round(this.NetAssetValue / this.SharesOutstanding, 5);
            foreach(ClientHolding holding in clientHoldings)
            {
                ClientHoldingValuation holdingValued = new ClientHoldingValuation(holding, this.GrossAssetValuePerShare);
                holdingValued.ApplyManagementFee(Math.Round((holding.Units / this.SharesOutstanding) *this.ManagementFeeAmount,2)); // this is the weighted average fee
                this.ClientHoldings.Add(holdingValued);
            }
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
        public bool IsValuedBase { get; set; } // This determines whether fxRates and Market prices are available on this date...

        //public decimal TotalPnl {get;} this is unrealised *base
        public SecurityPositionValuation(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            this.Position = position;
            this.AsOfDate = asOfDate;

            // these are temporary but i used this to make sure its valued..
            bool hasFxValue;
            bool hasSecurityValue;
            if (position.security.Currency.Symbol.ToString() == FundBaseCurrency)
            {
                hasFxValue = true;
                this.fxRate = decimal.One;
            }
            else
            {
                string fxSymbol = $"{position.security.Currency}{FundBaseCurrency}";
                ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
                hasFxValue = priceTable.ContainsKey(tableKeyFx);
                this.fxRate = (priceTable.ContainsKey(tableKeyFx)) ? priceTable[tableKeyFx] : decimal.One;
            }

            ValueTuple<string, DateTime> tableKeySecurity = (position.security.Symbol, asOfDate);
            hasSecurityValue = priceTable.ContainsKey(tableKeySecurity);
            this.price = priceTable.ContainsKey(tableKeySecurity) ? priceTable[tableKeySecurity] : decimal.Zero;

            this.IsValuedBase = (hasFxValue && hasSecurityValue);

            int multiplierPnL = (position.NetQuantity >= 0) ? 1 : -1;

            // raise exception if no fx rate is found.. (This might not be useful because USDUSD shouldnt be found...)
            this.MarketValueLocal = Math.Round(position.NetQuantity * price, 2);
            this.MarketValueBase = Math.Round(this.MarketValueLocal * fxRate, 2);
            this.unrealisedPnl = Math.Round(position.NetQuantity * (this.price - position.AverageCost) * multiplierPnL, 2);
        }
    }
    public class CashPositionValuation
    {
        public CalculatedCashPosition CashPosition { get; set; }
        public decimal fxRate { get; set; }
        public decimal MarketValueBase { get; set; }
        public bool IsValuedBase { get; set; } // This determines whether fxRates and Market prices are available on this date...
        public DateTime AsOfDate { get; set; }

        // Investigate.. you can have unrealised cash on cash balances...
        public CashPositionValuation(CalculatedCashPosition cashPosition, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {

            this.AsOfDate = asOfDate;
            // this is for cash holdings I can get the valuation in base currency
            this.CashPosition = cashPosition;
            if (cashPosition.currency.Symbol.ToString() == FundBaseCurrency)
            {
                IsValuedBase = true;
                this.fxRate = decimal.One;
            }
            else
            {
                string fxSymbol = $"{cashPosition.currency}{FundBaseCurrency}";
                ValueTuple<string, DateTime> tableKeyFx = (fxSymbol, asOfDate);
                this.IsValuedBase = priceTable.ContainsKey(tableKeyFx);
                this.fxRate = (priceTable.ContainsKey(tableKeyFx)) ? priceTable[tableKeyFx] : decimal.One;
            }
            this.MarketValueBase = Math.Round(this.CashPosition.AccountBalance * fxRate, 2);
        }
    }
    public class ClientHoldingValuation
    {
        public ClientHolding Holding { get; set; }
        public decimal GrossValuation { get;  set; }
        public decimal NetValuation { get; set; }
        public decimal ManagementFeesAccrued { get; set; }
        public decimal GavPrice { get; set; }

        public decimal PerformanceFeesAccrued { get; set; }
        public ClientHoldingValuation(ClientHolding clientHolding, decimal gavPrice)
        {
            this.Holding = clientHolding;
            this.GavPrice = gavPrice;
            this.GrossValuation = this.Holding.Units * this.GavPrice;
            this.NetValuation = this.Holding.Units * this.GavPrice;
            this.PerformanceFeesAccrued = 0;
            this.ManagementFeesAccrued = 0;
        }

        public void ApplyManagementFee(decimal fee)
        {
            ManagementFeesAccrued += fee;
            NetValuation -= fee;
            
        }
        public void ApplyPerformanceFee(decimal fee)
        {
            PerformanceFeesAccrued += fee;
            NetValuation -= fee;

        }
    }
    
}

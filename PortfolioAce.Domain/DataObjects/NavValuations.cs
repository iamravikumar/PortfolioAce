using PortfolioAce.Domain.DataObjects.PositionData;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.FactTables;
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
        // ONE MORE THING... There might be a potential big bug. If there is a Sub/Red that is not final Do i include the class but dont increase the SharesOutstanding
        // What i do is calculate the NAV price then mint the new units and amend the tranfer angency
        // i.e. there is a not final 50k subscription. i calculate the NAV price then set a units and price for that subscription.. this will cause the nav to stay the same and i can incorporate it..
        
        // performance fees will have to be calculated monthly for now.
        
        public Fund fund { get; set; }
        public decimal ManagementFeeAmount { get; set; }
        public decimal PerformanceFeeAmount { get; set; }
        public List<ValuedSecurityPosition> SecurityPositions { get; set; } // when i put this in a datagrid i can check what is fullyvalued from what isn't
        public List<CashPositionValuation> CashPositions { get; set; } // when i put this in a datagrid i can check what is fullyvalued from what isn't
        public List<ClientHoldingValuation> ClientHoldings { get; set; }
        public DateTime AsOfDate { get; set; }
        public decimal NetAssetValue { get; set; }
        public decimal GrossAssetValue { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal NetAssetValuePerShare { get; set; }
        public decimal GrossAssetValuePerShare { get; set; }
        public int UnvaluedPositions { get; set; } // number of positions that do not have a price...
        public NavValuations(List<ValuedSecurityPosition> securityPositions, List<CashPositionValuation> cashPositions, List<ClientHolding> clientHoldings,
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
            this.NetAssetValue = GrossAssetValue - this.ManagementFeeAmount; // this is the NAV after management fees.
            this.NetAssetValuePerShare = this.NetAssetValue / this.SharesOutstanding; // NAV per share after management fees

            NAVPriceStoreFACT performancePeriodFact = fund.NavPrices.Where(np => np.FinalisedDate.Month == AsOfDate.Month).OrderBy(np => np.FinalisedDate).FirstOrDefault(); //price at beginning of month
            decimal performancePeriodStartPrice = (performancePeriodFact==null)?this.NetAssetValuePerShare:performancePeriodFact.NAVPrice; // if theres no price at the beginning of month this value is the beginning of month... 

            decimal totalPerfFee = decimal.Zero;
            foreach (ClientHolding holding in clientHoldings)
            {
                decimal holdingWeighting = holding.Units / this.SharesOutstanding;
                decimal perfFee = decimal.Zero;
                decimal perfStartPrice;
                // This section determines the start price to use for the performance fee.
                if (holding.Investor.HighWaterMark == null)
                {
                    perfStartPrice = performancePeriodStartPrice;
                }
                else if (holding.Investor.HighWaterMark > this.NetAssetValuePerShare)
                {
                    perfStartPrice = this.NetAssetValue; // AKA no performance
                }
                else
                {
                    perfStartPrice = (decimal)holding.Investor.HighWaterMark;
                }
                //6/5 20%. how caculate gain...
                decimal gainPercent = (this.NetAssetValuePerShare / perfStartPrice) - 1;
                decimal gainValue = (this.NetAssetValuePerShare * holding.Units) - (perfStartPrice * holding.Units);

                if (gainPercent > 0)
                {
                    //hurdle calc
                    if (fund.HurdleType == "None")
                    {
                        perfFee = gainValue * (fund.PerformanceFee / 12); // by 12 since the fee is payable monthly for now...
                    }
                    else if (fund.HurdleType == "Soft" && gainPercent >= fund.HurdleRate)
                    {
                        perfFee = gainValue * (fund.PerformanceFee / 12);
                    }
                    else if (fund.HurdleType == "Hard" && gainPercent > fund.HurdleRate)
                    {
                        decimal gainPercentWithHurdle = gainPercent - fund.HurdleRate;

                        decimal gainWithHurdle = (this.NetAssetValuePerShare * holding.Units) - (perfStartPrice * holding.Units * (1 + fund.HurdleRate));
                        perfFee = gainWithHurdle * (fund.PerformanceFee / 12);
                    }
                }


                totalPerfFee += perfFee;

                ClientHoldingValuation holdingValued = new ClientHoldingValuation(holding, this.GrossAssetValuePerShare);
                holdingValued.ApplyManagementFee(Math.Round(holdingWeighting * this.ManagementFeeAmount,2)); // this is the weighted average fee
                holdingValued.ApplyPerformanceFee(Math.Round(perfFee, 2));
                this.ClientHoldings.Add(holdingValued);
            }

            this.PerformanceFeeAmount = totalPerfFee;
            this.NetAssetValue = this.NetAssetValue - totalPerfFee;
            this.NetAssetValuePerShare = Math.Round(this.NetAssetValue / this.SharesOutstanding, 5);

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
            if (cashPosition.currency.Symbol == FundBaseCurrency)
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

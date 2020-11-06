using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore;
using PortfolioAce.EFCore.Repository;
using System;
using System.Linq;
using System.Reflection.Metadata;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check the file in this location
            // C:\Users\Owner\Documents\PortfolioAce\PortfolioAce\ConsoleApp1\bin\Debug\netcoreapp3.1
            IFundRepository repo = new FundRepository(new PortfolioAceDbContextFactory());
            /*
            repo.CreateFund(new Fund 
            {
                FundName="Pace",
                Symbol="PA",
                BaseCurrency="GBP",
                ManagementFee=0.2m,
                PerformanceFee=0.2m,
                NAVFrequency="Monthly"
            }).Wait();
            */
            //var y = repo.GetAllFunds();
            var x = repo.GetFund("PA");
            /*
            x.FundName = "PacMan";
            repo.UpdateFund(x);
            */
            /*
            repo.DeleteFund(1);
            */
            Console.WriteLine(x);

            /*
            ICashTradeRepository Crepo = new CashTradeRepository(new PortfolioAceDbContextFactory());
            Crepo.CreateCashTrade(new CashTrade
            {
                Type = "Expense",
                Amount = 15000m,
                TradeDate = new DateTime(2020, 10, 3),
                SettleDate = new DateTime(2020, 10, 3),
                Comment = "General Tax",
                Currency = "EUR",
                FundId = 1,
            }).Wait();
            Crepo.CreateCashTrade(new CashTrade
            {
                Type = "Income",
                Amount = 56.24m,
                TradeDate = new DateTime(2020, 10, 4),
                SettleDate = new DateTime(2020, 10, 4),
                Comment = "Tax Rebate",
                Currency = "GBP",
                FundId = 1,
            }).Wait();
            
            
            ITradeRepository Trepo = new TradeRepository(new PortfolioAceDbContextFactory());
            Trepo.CreateTrade(new Trade
            {
                TradeType = "Trade",
                Symbol = "MSFT",
                Quantity = 500m,
                Price = 54.2m,
                TradeAmount = -25000m,
                TradeDate = new DateTime(2020, 10, 1),
                SettleDate = new DateTime(2020, 10, 1),
                Currency = "EUR",
                Commission = 36.54m,
                FundId = 1
            });
            Trepo.CreateTrade(new Trade
            {
                TradeType = "Trade",
                Symbol = "MSFT",
                Quantity = -500m,
                Price = 53.2m,
                TradeAmount = 24732.12m,
                TradeDate = new DateTime(2020, 10, 2),
                SettleDate = new DateTime(2020, 10, 2),
                Currency = "EUR",
                Commission = 36.54m,
                FundId = 1
            });
            */

            /*
            IAdminRepository Arepo = new AdminRepository(new PortfolioAceDbContextFactory());
            Arepo.AddSecurityInfo(new Security
            {
                Symbol = "MSFT",
                SecurityName = "Microsoft",
                Type = "Equity",
                Currency = "USD"
            });
            */
        }
    }
}

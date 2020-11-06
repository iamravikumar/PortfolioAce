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
                FundName="Pace1",
                Symbol="PA",
                BaseCurrency="GBP",
                ManagementFee=0.2m,
                PerformanceFee=0.2m,
                NAVFrequency="Monthly"
            }).Wait();
            */
            var y = repo.GetAllFunds();
            var x = repo.GetFund("PA");
            /*
            x.FundName = "PacMan";
            repo.UpdateFund(x);
            */
            /*
            repo.DeleteFund(1);
            */
            Console.WriteLine(x);
        }
    }
}

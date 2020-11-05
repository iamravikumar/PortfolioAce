using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore;
using PortfolioAce.EFCore.Repository;
using System;
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
            repo.Create(new Fund 
            {
                FundName="Pace",
                Symbol="PA",
                BaseCurrency="GBP",
                ManagementFee=0.2m,
                PerformanceFee=0.2m,
                NAVFrequency="Monthly"
            }).Wait();
        }
    }
}

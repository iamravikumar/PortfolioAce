using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects
{
    public class SeedingInvestor
    {
        public string InvestorName { get; set; }
        public decimal SeedAmount { get; set; }
        public SeedingInvestor(string InvestorName, decimal SeedAmount)
        {
            this.InvestorName = InvestorName;
            this.SeedAmount = SeedAmount;
        }
        public SeedingInvestor()
        {

        }
    }
}

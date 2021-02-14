namespace PortfolioAce.HelperObjects
{
    public class SeedingInvestor
    {
        public int InvestorId { get; set; }
        public decimal SeedAmount { get; set; }
        public SeedingInvestor(int InvestorId, decimal SeedAmount)
        {
            this.InvestorId = InvestorId;
            this.SeedAmount = SeedAmount;
        }
        public SeedingInvestor()
        {

        }
    }
}

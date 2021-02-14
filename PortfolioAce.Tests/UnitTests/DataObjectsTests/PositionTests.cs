using PortfolioAce.Tests.HelperClasses;
using System.Linq;
using Xunit;

namespace PortfolioAce.Tests.UnitTests.DataObjectsTests
{
    public class PositionTests : IClassFixture<PortfolioSeedDataFixture>
    {

        PortfolioSeedDataFixture fixture;

        public PositionTests(PortfolioSeedDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void DummyTest()
        {
            var x = fixture.PortfolioContext.Securities.ToList();
            Assert.True(true);
        }

        [Fact]
        public void DummyTest2()
        {
            var x = fixture.PortfolioContext.Securities.ToList();
            Assert.True(true);
        }
    }
}

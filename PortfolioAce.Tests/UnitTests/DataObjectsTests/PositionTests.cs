using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore;
using PortfolioAce.EFCore.Services;
using PortfolioAce.Tests.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PortfolioAce.Tests.UnitTests.DataObjectsTests
{
    public class PositionTests:IClassFixture<PortfolioSeedDataFixture>
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

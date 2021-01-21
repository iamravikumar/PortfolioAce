using Microsoft.EntityFrameworkCore;
using PortfolioAce.EFCore;
using PortfolioAce.EFCore.Services;
using PortfolioAce.Tests.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PortfolioAce.Tests.IntegrationTests
{
    public class DummyServiceTest
    {
        // Databases for these tests should have different names.
        // find a way to create a new factory
        [Fact]
        public void DummyServiceTest1()
        {
            Action<DbContextOptionsBuilder> configureDbContext = db => db.UseInMemoryDatabase(databaseName: "DummyServiceTest1");
            PortfolioAceDbContextFactory factory = new PortfolioAceDbContextFactory(configureDbContext);
            using (PortfolioAceDbContext context = factory.CreateDbContext())
            {
                TestSeeds.SeedPortfolio(context);
            }
            IAdminService aService = new AdminService(factory);
            bool res = aService.SecurityExists("AAPL");
            Assert.True(res);
        }

        [Fact]
        public void DummyServiceTest2()
        {
            Action<DbContextOptionsBuilder> configureDbContext = db => db.UseInMemoryDatabase(databaseName: "DummyServiceTest2");
            PortfolioAceDbContextFactory factory = new PortfolioAceDbContextFactory(configureDbContext);
            using (PortfolioAceDbContext context = factory.CreateDbContext())
            {
                TestSeeds.SeedPortfolio(context);
            }
            IAdminService aService = new AdminService(factory);
            bool res = aService.SecurityExists("MSFT");
            Assert.True(res);
        }
    }
}

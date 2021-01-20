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
    public class PositionTests
    {
        // All databases should have different names.
        [Fact]
        public void ADBContextFactory()
        {
            Action<DbContextOptionsBuilder> configureDbContext = db => db.UseInMemoryDatabase(databaseName: "ShouldReturnAllCustomers");

            var factory = new PortfolioAceDbContextFactory(configureDbContext);
            using (PortfolioAceDbContext context = factory.CreateDbContext())
            {
                TestSeeds.Seed(context);
            }
            IAdminService aService = new AdminService(factory);
            bool res = aService.SecurityExists("AAPL");
            Assert.True(res);
        }

        [Fact]
        public void ADBContextFactory2()
        {
            Action<DbContextOptionsBuilder> configureDbContext = db => db.UseInMemoryDatabase(databaseName: "ShouldReturnAllCustomers2");

            var factory = new PortfolioAceDbContextFactory(configureDbContext);
            using (PortfolioAceDbContext context = factory.CreateDbContext())
            {
                TestSeeds.Seed(context);
            }
            IAdminService aService = new AdminService(factory);
            bool res = aService.SecurityExists("AAPL");
            Assert.True(res);
        }

    }
}

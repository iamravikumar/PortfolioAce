using Microsoft.EntityFrameworkCore;
using PortfolioAce.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Tests.HelperClasses
{
    public class PortfolioSeedDataFixture : IDisposable
    {
        public PortfolioAceDbContext PortfolioContext { get; private set; }

        public PortfolioSeedDataFixture()
        {
            // The Database should have a different name each time..
            Action<DbContextOptionsBuilder> configureDbContext = db => db.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            PortfolioAceDbContextFactory factory = new PortfolioAceDbContextFactory(configureDbContext);
            PortfolioContext = factory.CreateDbContext();
            TestSeeds.SeedPortfolio(PortfolioContext);
        }

        public void Dispose()
        {
            PortfolioContext.Dispose();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;

namespace PortfolioAce.EFCore
{
    // allows us to use multiple db context
    public class PortfolioAceDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public PortfolioAceDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public PortfolioAceDbContext CreateDbContext(string[] args = null)
        {

            var options = new DbContextOptionsBuilder<PortfolioAceDbContext>();

            _configureDbContext(options);
            return new PortfolioAceDbContext(options.Options);
        }
    }
}

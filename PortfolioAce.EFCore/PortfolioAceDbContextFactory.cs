using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

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

        public PortfolioAceDbContext CreateDbContext(string[] args=null)
        {
            
            var options = new DbContextOptionsBuilder<PortfolioAceDbContext>();

            _configureDbContext(options);
            return new PortfolioAceDbContext(options.Options);
        }
    }
}

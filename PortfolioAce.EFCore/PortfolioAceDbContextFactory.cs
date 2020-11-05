using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.EFCore
{
    // allows us to use multiple db context
    public class PortfolioAceDbContextFactory: IDesignTimeDbContextFactory<PortfolioAceDbContext>
    {
        public PortfolioAceDbContext CreateDbContext(string[] args=null)
        {
            var options = new DbContextOptionsBuilder<PortfolioAceDbContext>();
            options.UseSqlite("Data Source=PortfolioAce.db");
            return new PortfolioAceDbContext(options.Options);
        }
    }
}

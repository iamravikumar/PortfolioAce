using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.EFCore
{
    class PortfolioAceDbContextFactory: IDesignTimeDbContextFactory<PortfolioAceDbContext>
    {
        public PortfolioAceDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<PortfolioAceDbContext>();
            options.UseSqlite("Data Source=PortfolioAce.db");
            return new PortfolioAceDbContext(options.Options);
        }
    }
}

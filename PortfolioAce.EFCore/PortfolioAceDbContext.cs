using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.EFCore
{
    // this class will manage our interaction with the database.
    public class PortfolioAceDbContext:DbContext
    {
        public DbSet<Fund> Funds { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<CashTrade> CashTrades { get; set; }
        public DbSet<CashBook> CashBooks { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<TransferAgency> TransferAgent { get; set; }

        public PortfolioAceDbContext(DbContextOptions options) : base(options)
        { 
        }

        // I can use on model creating to seed data


    }
}

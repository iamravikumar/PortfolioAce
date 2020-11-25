using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
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
        public DbSet<NAVPriceStore> NavPriceData { get; set; }
        public DbSet<SecurityPriceStore> SecurityPriceData { get; set; }

        // Back Office Models
        public DbSet<TradeBO> Trades_ { get; set; }
        public DbSet<CashTradeBO> CashTrades_ { get; set; }
        public DbSet<CashBookBO> CashBooks_ { get; set; }
        public DbSet<TransferAgencyBO> TransferAgent_ { get; set; }

        // Dimensions
        public DbSet<SecuritiesDIM> Securities_ { get; set; }
        public DbSet<AssetClassDIM> AssetClasses_ { get; set; }
        public DbSet<CashTradeTypesDIM> CashTradeTypes_ { get; set; }
        public DbSet<CurrenciesDIM> Currencies_ { get; set; }
        public DbSet<IssueTypesDIM> IssueTypes_ { get; set; }
        public DbSet<NavFrequencyDIM> NavFrequencies_ { get; set; }
        public DbSet<TradeTypesDIM> TradeTypes_ { get; set; }


        // Fact Tables
        public DbSet<NAVPriceStoreFACT> NavPriceData_ { get; set; }
        public DbSet<FundPerformanceFACT> FundPerformance_ { get; set; }
        public DbSet<PositionFACT> Positions_ { get; set; }
        //public DbSet<SecurityPriceStore> SecurityPrices { get; set; }

        public PortfolioAceDbContext(DbContextOptions options) : base(options)
        { 
        }

        // I can use on model creating to seed data


    }
}

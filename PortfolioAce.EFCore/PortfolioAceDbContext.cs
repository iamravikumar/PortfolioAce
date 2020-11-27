﻿using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.Domain.ModelSeedData;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.EFCore
{
    // this class will manage our interaction with the database.
    public class PortfolioAceDbContext:DbContext
    {
        public DbSet<Fund> Funds { get; set; }
        //public DbSet<Security> Securities { get; set; }
        public DbSet<SecurityPriceStore> SecurityPriceData { get; set; }

        // Back Office Models
        public DbSet<TradeBO> Trades { get; set; }
        public DbSet<CashTradeBO> CashTrades { get; set; }
        public DbSet<CashBookBO> CashBooks { get; set; }
        public DbSet<TransferAgencyBO> TransferAgent { get; set; }

        // Dimensions
        public DbSet<SecuritiesDIM> Securities { get; set; }
        public DbSet<AssetClassDIM> AssetClasses { get; set; }
        public DbSet<CashTradeTypesDIM> CashTradeTypes { get; set; }
        public DbSet<CurrenciesDIM> Currencies { get; set; }
        public DbSet<IssueTypesDIM> IssueTypes { get; set; }
        public DbSet<NavFrequencyDIM> NavFrequencies { get; set; }
        public DbSet<TradeTypesDIM> TradeTypes { get; set; }


        // Fact Tables
        public DbSet<NAVPriceStoreFACT> NavPriceData { get; set; }
        public DbSet<FundPerformanceFACT> FundPerformance { get; set; }
        public DbSet<PositionFACT> Positions { get; set; }
        //public DbSet<SecurityPriceStore> SecurityPrices { get; set; }

        public PortfolioAceDbContext(DbContextOptions options) : base(options)
        { 
        }

        // I can use on model creating to seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Initial Seed Data for dimensions
            SeedData seedData = new SeedData();
            modelBuilder.Entity<AssetClassDIM>().HasData(
                seedData.SeedAssetClasses);
            modelBuilder.Entity<CashTradeTypesDIM>().HasData(
                seedData.SeedCashTradeTypes);

            modelBuilder.Entity<CurrenciesDIM>().HasData(
                seedData.SeedCurrencies);
            modelBuilder.Entity<IssueTypesDIM>().HasData(
                seedData.SeedIssueTypes);

            modelBuilder.Entity<NavFrequencyDIM>().HasData(
                seedData.SeedNavFrequencies);
            modelBuilder.Entity<TradeTypesDIM>().HasData(
                seedData.SeedTradeTypes);
            base.OnModelCreating(modelBuilder);
        }

    }
}

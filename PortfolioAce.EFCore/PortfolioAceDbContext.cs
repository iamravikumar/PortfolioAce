using Microsoft.EntityFrameworkCore;
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
        public DbSet<SecurityPriceStore> SecurityPriceData { get; set; }

        // Back Office Models
        public DbSet<TransferAgencyBO> TransferAgent { get; set; }
        public DbSet<TransactionsBO> Transactions { get; set; }

        // Dimensions
        public DbSet<SecuritiesDIM> Securities { get; set; }
        public DbSet<AssetClassDIM> AssetClasses { get; set; }
        public DbSet<CurrenciesDIM> Currencies { get; set; }
        public DbSet<IssueTypesDIM> IssueTypes { get; set; }
        public DbSet<NavFrequencyDIM> NavFrequencies { get; set; }
        public DbSet<CustodiansDIM> Custodians { get; set; }
        public DbSet<AccountingPeriodsDIM> Periods { get; set; }
        public DbSet<TransactionTypeDIM> TransactionTypes { get; set; }

        // Fact Tables
        public DbSet<NAVPriceStoreFACT> NavPriceData { get; set; }
        public DbSet<FundPerformanceFACT> FundPerformance { get; set; }
        public DbSet<PositionFACT> Positions { get; set; }

        public PortfolioAceDbContext(DbContextOptions options) : base(options)
        { 
        }

        // I can use on model creating to seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Enum converters
            modelBuilder.Entity<CurrenciesDIM>()
                .Property(c => c.Name)
                .HasConversion<string>();
            modelBuilder.Entity<CurrenciesDIM>()
                .Property(c => c.Symbol)
                .HasConversion<string>();
            modelBuilder.Entity<TransactionTypeDIM>()
                .Property(t=>t.TypeClass)
                .HasConversion<string>();
            modelBuilder.Entity<TransactionTypeDIM>()
                .Property(t => t.TypeName)
                .HasConversion<string>();
            modelBuilder.Entity<TransactionTypeDIM>()
                .Property(t => t.Direction)
                .HasConversion<string>();
            modelBuilder.Entity<AssetClassDIM>()
                .Property(a => a.Name)
                .HasConversion<string>();

            // Initial Seed Data for dimensions
            SeedData seedData = new SeedData();
            modelBuilder.Entity<AssetClassDIM>().HasData(
                seedData.SeedAssetClasses);

            modelBuilder.Entity<CurrenciesDIM>().HasData(
                seedData.SeedCurrencies);
            modelBuilder.Entity<IssueTypesDIM>().HasData(
                seedData.SeedIssueTypes);

            modelBuilder.Entity<NavFrequencyDIM>().HasData(
                seedData.SeedNavFrequencies);
            modelBuilder.Entity<CustodiansDIM>().HasData(
                seedData.SeedCustodians);
            modelBuilder.Entity<TransactionTypeDIM>().HasData(
                seedData.SeedTransactionTypes);
            modelBuilder.Entity<SecuritiesDIM>().HasData(
                seedData.seedSecuritisedCash);
            
            base.OnModelCreating(modelBuilder);
        }

    }
}

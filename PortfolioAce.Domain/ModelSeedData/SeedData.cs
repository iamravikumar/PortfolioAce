using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Domain.ModelSeedData
{
    // This represents the initial seed data for my objects. Its a singleton
    public class SeedData
    {
        public readonly CurrenciesDIM[] SeedCurrencies = new CurrenciesDIM[] {
            new CurrenciesDIM {CurrencyId=1, Name="Pound Sterling", Symbol="GBP"},
            new CurrenciesDIM {CurrencyId=2, Name="Euro", Symbol="EUR"},
            new CurrenciesDIM {CurrencyId=3, Name="United States Dollar", Symbol="USD"},
            new CurrenciesDIM {CurrencyId=4, Name="Japanese Yen", Symbol="JPY"},
            new CurrenciesDIM {CurrencyId=5, Name="Swiss Franc", Symbol="CHF"},
            new CurrenciesDIM {CurrencyId=6, Name="Canadian Dollar", Symbol="CAD"},
            new CurrenciesDIM {CurrencyId=7, Name="Australian Dollar", Symbol="AUD"}
        };

        public readonly AssetClassDIM[] SeedAssetClasses = new AssetClassDIM[]{
            new AssetClassDIM {AssetClassId=1, Name="Crytocurrency"},
            new AssetClassDIM {AssetClassId=2, Name="Equity"},
            new AssetClassDIM {AssetClassId=3, Name="FX"}
        };

        public readonly NavFrequencyDIM[] SeedNavFrequencies = new NavFrequencyDIM[]{
            new NavFrequencyDIM{NavFrequencyId=1, Frequency="Daily"},
            new NavFrequencyDIM{NavFrequencyId=2, Frequency="Monthly"}
        };

        public readonly TradeTypesDIM[] SeedTradeTypes = new TradeTypesDIM[]{
            new TradeTypesDIM{SecurityTypeId=1, TypeName="Security Trade"},
            new TradeTypesDIM{SecurityTypeId=2, TypeName="Corporate Action"}
        };

        public readonly CashTradeTypesDIM[] SeedCashTradeTypes = new CashTradeTypesDIM[]{
            new CashTradeTypesDIM{CashTypeId=1, TypeName="Income"},
            new CashTradeTypesDIM{CashTypeId=2, TypeName="Expense"}
        };

        public readonly IssueTypesDIM[] SeedIssueTypes = new IssueTypesDIM[]{
            new IssueTypesDIM{IssueTypeID=1, TypeName="Subscription"},
            new IssueTypesDIM{IssueTypeID=2, TypeName="Redemption"}
        };
    }
}

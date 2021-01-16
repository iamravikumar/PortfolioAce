using PortfolioAce.Domain.Models;
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
            new CurrenciesDIM {CurrencyId=1, Name="PoundSterling", Symbol="GBP"},
            new CurrenciesDIM {CurrencyId=2, Name="Euro", Symbol="EUR"},
            new CurrenciesDIM {CurrencyId=3, Name="UnitedStatesDollar", Symbol="USD"},
            new CurrenciesDIM {CurrencyId=4, Name="JapaneseYen", Symbol="JPY"},
            new CurrenciesDIM {CurrencyId=5, Name="IndianRupee", Symbol="INR"},
            new CurrenciesDIM {CurrencyId=6, Name="SwissFranc", Symbol="CHF"},
            new CurrenciesDIM {CurrencyId=7, Name="CanadianDollar", Symbol="CAD"},
            new CurrenciesDIM {CurrencyId=8, Name="AustralianDollar", Symbol="AUD"}
        };

        public readonly TransactionTypeDIM[] SeedTransactionTypes = new TransactionTypeDIM[]
        {
            new TransactionTypeDIM {TransactionTypeId=1, TypeName="Trade", TypeClass="SecurityTrade", Direction="None"},
            new TransactionTypeDIM {TransactionTypeId=2, TypeName="Dividends", TypeClass="SecurityTrade", Direction="None"},
            new TransactionTypeDIM {TransactionTypeId=3, TypeName="Income", TypeClass="CashTrade", Direction="Inflow"},
            new TransactionTypeDIM {TransactionTypeId=4, TypeName="Expense", TypeClass="CashTrade", Direction="Outflow"},
            new TransactionTypeDIM {TransactionTypeId=5, TypeName="Deposit", TypeClass="CashTrade", Direction="Inflow"},
            new TransactionTypeDIM {TransactionTypeId=6, TypeName="Withdrawal", TypeClass="CashTrade", Direction="Outflow"},
            new TransactionTypeDIM {TransactionTypeId=7, TypeName="Interest", TypeClass="CashTrade", Direction="None"},
            new TransactionTypeDIM {TransactionTypeId=8, TypeName="ManagementFee", TypeClass="CashTrade", Direction="Outflow"},
            new TransactionTypeDIM {TransactionTypeId=9, TypeName="PerformanceFee", TypeClass="CashTrade", Direction="Outflow"},
            new TransactionTypeDIM {TransactionTypeId=10, TypeName="Miscellaneous", TypeClass="CashTrade", Direction="None"},
            new TransactionTypeDIM {TransactionTypeId=11, TypeName="FXBuy", TypeClass="CashTrade", Direction="Inflow"},
            new TransactionTypeDIM {TransactionTypeId=12, TypeName="FXSell", TypeClass="CashTrade", Direction="Outflow"},
            new TransactionTypeDIM {TransactionTypeId=13, TypeName="FXTrade", TypeClass="FXTrade", Direction="None"},
            new TransactionTypeDIM {TransactionTypeId=14, TypeName="FXTradeCollapse", TypeClass="FXTrade", Direction="None"}
        };

        public readonly AssetClassDIM[] SeedAssetClasses = new AssetClassDIM[]{
            new AssetClassDIM {AssetClassId=1, Name="Equity"},
            new AssetClassDIM {AssetClassId=2, Name="Cryptocurrency"},
            new AssetClassDIM {AssetClassId=3, Name="FX"},
            new AssetClassDIM {AssetClassId=4, Name="Cash"},
            new AssetClassDIM {AssetClassId=5, Name="InterestRate"},
        };

        public readonly SecuritiesDIM[] seedSecuritisedCash = new SecuritiesDIM[]
        {
            new SecuritiesDIM{SecurityId=1, SecurityName="CASH GBP", Symbol="GBPc", AssetClassId=4, CurrencyId=1},
            new SecuritiesDIM{SecurityId=2, SecurityName="CASH EURO", Symbol="EURc", AssetClassId=4, CurrencyId=2},
            new SecuritiesDIM{SecurityId=3, SecurityName="CASH USD", Symbol="USDc", AssetClassId=4, CurrencyId=3},
            new SecuritiesDIM{SecurityId=4, SecurityName="CASH JPY", Symbol="JPYc", AssetClassId=4, CurrencyId=4},
            new SecuritiesDIM{SecurityId=5, SecurityName="CASH INR", Symbol="INRc", AssetClassId=4, CurrencyId=5},
            new SecuritiesDIM{SecurityId=6, SecurityName="CASH CHF", Symbol="CHFc", AssetClassId=4, CurrencyId=6},
            new SecuritiesDIM{SecurityId=7, SecurityName="CASH CAD", Symbol="CADc", AssetClassId=4, CurrencyId=7},
            new SecuritiesDIM{SecurityId=8, SecurityName="CASH AUD", Symbol="AUDc", AssetClassId=4, CurrencyId=8},
        };

        public readonly NavFrequencyDIM[] SeedNavFrequencies = new NavFrequencyDIM[]{
            new NavFrequencyDIM{NavFrequencyId=1, Frequency="Daily"},
            new NavFrequencyDIM{NavFrequencyId=2, Frequency="Monthly"}
        };

        public readonly IssueTypesDIM[] SeedIssueTypes = new IssueTypesDIM[]{
            new IssueTypesDIM{IssueTypeID=1, TypeName="Subscription"},
            new IssueTypesDIM{IssueTypeID=2, TypeName="Redemption"}
        };

        public readonly CustodiansDIM[] SeedCustodians = new CustodiansDIM[]{
            new CustodiansDIM{CustodianId=1, Name="Default", Symbol="Default"}
        };
        public readonly ApplicationSettings[] SeedSettings = new ApplicationSettings[]{
            new ApplicationSettings{SettingId=1, SettingName="AlphaVantageAPI", Description="Alpha Vantage API Key", SettingValue="demo"},
            new ApplicationSettings{SettingId=2, SettingName="FMPrepAPI", Description="Financial Modelling Prep API Key", SettingValue="demo"}
        };
    }
}

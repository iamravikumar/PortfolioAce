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
            new CurrenciesDIM {CurrencyId=1, Name=ISOName.PoundSterling, Symbol=ISOSymbol.GBP},
            new CurrenciesDIM {CurrencyId=2, Name=ISOName.Euro, Symbol=ISOSymbol.EUR},
            new CurrenciesDIM {CurrencyId=3, Name=ISOName.UnitedStatesDollar, Symbol=ISOSymbol.USD},
            new CurrenciesDIM {CurrencyId=4, Name=ISOName.JapaneseYen, Symbol=ISOSymbol.JPY},
            new CurrenciesDIM {CurrencyId=5, Name=ISOName.IndianRupee, Symbol=ISOSymbol.INR},
            new CurrenciesDIM {CurrencyId=6, Name=ISOName.SwissFranc, Symbol=ISOSymbol.CHF},
            new CurrenciesDIM {CurrencyId=7, Name=ISOName.CanadianDollar, Symbol=ISOSymbol.CAD},
            new CurrenciesDIM {CurrencyId=8, Name=ISOName.AustralianDollar, Symbol=ISOSymbol.AUD}
        };

        public readonly TransactionTypeDIM[] SeedTransactionTypes = new TransactionTypeDIM[]
        {
            new TransactionTypeDIM {TransactionTypeId=1, TypeName=TranTypes.Trade, TypeClass=TranClasses.SecurityTrade, Direction=TranDirection.None},
            new TransactionTypeDIM {TransactionTypeId=2, TypeName=TranTypes.Coupon, TypeClass=TranClasses.SecurityTrade, Direction=TranDirection.None},
            new TransactionTypeDIM {TransactionTypeId=3, TypeName=TranTypes.Dividends, TypeClass=TranClasses.SecurityTrade, Direction=TranDirection.None},
            new TransactionTypeDIM {TransactionTypeId=4, TypeName=TranTypes.Income, TypeClass=TranClasses.CashTrade, Direction=TranDirection.Inflow},
            new TransactionTypeDIM {TransactionTypeId=5, TypeName=TranTypes.Expense, TypeClass=TranClasses.CashTrade, Direction=TranDirection.Outflow},
            new TransactionTypeDIM {TransactionTypeId=6, TypeName=TranTypes.Deposit, TypeClass=TranClasses.CashTrade, Direction=TranDirection.Inflow},
            new TransactionTypeDIM {TransactionTypeId=7, TypeName=TranTypes.Withdrawal, TypeClass=TranClasses.CashTrade, Direction=TranDirection.Outflow},
            new TransactionTypeDIM {TransactionTypeId=8, TypeName=TranTypes.Interest, TypeClass=TranClasses.CashTrade, Direction=TranDirection.None},
            new TransactionTypeDIM {TransactionTypeId=9, TypeName=TranTypes.ManagementFee, TypeClass=TranClasses.CashTrade, Direction=TranDirection.Outflow},
            new TransactionTypeDIM {TransactionTypeId=10, TypeName=TranTypes.PerformanceFee, TypeClass=TranClasses.CashTrade, Direction=TranDirection.Outflow},
            new TransactionTypeDIM {TransactionTypeId=11, TypeName=TranTypes.Miscellaneous, TypeClass=TranClasses.CashTrade, Direction=TranDirection.None}
        };

        public readonly AssetClassDIM[] SeedAssetClasses = new AssetClassDIM[]{
            new AssetClassDIM {AssetClassId=1, Name=AssetClass.Equity},
            new AssetClassDIM {AssetClassId=2, Name=AssetClass.Crytocurrency},
            new AssetClassDIM {AssetClassId=3, Name=AssetClass.FX},
            new AssetClassDIM {AssetClassId=4, Name=AssetClass.Cash},
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

        public readonly TradeTypesDIM[] SeedTradeTypes = new TradeTypesDIM[]{
            new TradeTypesDIM{SecurityTypeId=1, TypeName="Security Trade"},
            new TradeTypesDIM{SecurityTypeId=2, TypeName="Corporate Action"}
        }; // to be Deleted

        public readonly CashTradeTypesDIM[] SeedCashTradeTypes = new CashTradeTypesDIM[]{
            new CashTradeTypesDIM{CashTypeId=1, TypeName="Income"},
            new CashTradeTypesDIM{CashTypeId=2, TypeName="Expense"}
        }; // To be Deleted

        public readonly IssueTypesDIM[] SeedIssueTypes = new IssueTypesDIM[]{
            new IssueTypesDIM{IssueTypeID=1, TypeName="Subscription"},
            new IssueTypesDIM{IssueTypeID=2, TypeName="Redemption"}
        };

        public readonly CustodiansDIM[] SeedCustodians = new CustodiansDIM[]{
            new CustodiansDIM{CustodianId=1, Name="Default", Symbol="Default"}
        };
    }
}

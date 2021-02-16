using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PortfolioAce.EFCore.Migrations
{
    public partial class reset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                columns: table => new
                {
                    SettingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SettingName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    SettingValue = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "bo_LinkedTrades",
                columns: table => new
                {
                    LinkedTradeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bo_LinkedTrades", x => x.LinkedTradeId);
                });

            migrationBuilder.CreateTable(
                name: "dim_AssetClasses",
                columns: table => new
                {
                    AssetClassId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_AssetClasses", x => x.AssetClassId);
                });

            migrationBuilder.CreateTable(
                name: "dim_Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "dim_Custodians",
                columns: table => new
                {
                    CustodianId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_Custodians", x => x.CustodianId);
                });

            migrationBuilder.CreateTable(
                name: "dim_IssueTypes",
                columns: table => new
                {
                    IssueTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_IssueTypes", x => x.IssueTypeID);
                });

            migrationBuilder.CreateTable(
                name: "dim_NavFrequencies",
                columns: table => new
                {
                    NavFrequencyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Frequency = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_NavFrequencies", x => x.NavFrequencyId);
                });

            migrationBuilder.CreateTable(
                name: "dim_TransactionType",
                columns: table => new
                {
                    TransactionTypeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(nullable: false),
                    TypeClass = table.Column<string>(nullable: false),
                    Direction = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_TransactionType", x => x.TransactionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "fact_FundPerformance",
                columns: table => new
                {
                    PerformanceID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fact_FundPerformance", x => x.PerformanceID);
                });

            migrationBuilder.CreateTable(
                name: "Fund",
                columns: table => new
                {
                    FundId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FundName = table.Column<string>(nullable: false),
                    Symbol = table.Column<string>(nullable: false),
                    MinimumInvestment = table.Column<decimal>(nullable: false),
                    BaseCurrency = table.Column<string>(maxLength: 3, nullable: false),
                    ManagementFee = table.Column<decimal>(type: "decimal(6,4)", nullable: false),
                    PerformanceFee = table.Column<decimal>(type: "decimal(6,4)", nullable: false),
                    HurdleRate = table.Column<decimal>(type: "decimal(6,4)", nullable: false),
                    HurdleType = table.Column<string>(nullable: false),
                    HasHighWaterMark = table.Column<bool>(nullable: false),
                    NAVFrequency = table.Column<string>(nullable: false),
                    LaunchDate = table.Column<DateTime>(nullable: false),
                    IsInitialised = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fund", x => x.FundId);
                });

            migrationBuilder.CreateTable(
                name: "Investors",
                columns: table => new
                {
                    InvestorId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Domicile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    NativeLanguage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investors", x => x.InvestorId);
                });

            migrationBuilder.CreateTable(
                name: "dim_Securities",
                columns: table => new
                {
                    SecurityId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(nullable: false),
                    AlphaVantageSymbol = table.Column<string>(nullable: true),
                    FMPSymbol = table.Column<string>(nullable: true),
                    SecurityName = table.Column<string>(nullable: false),
                    ISIN = table.Column<string>(nullable: true),
                    AssetClassId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_Securities", x => x.SecurityId);
                    table.ForeignKey(
                        name: "FK_dim_Securities_dim_AssetClasses_AssetClassId",
                        column: x => x.AssetClassId,
                        principalTable: "dim_AssetClasses",
                        principalColumn: "AssetClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dim_Securities_dim_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "dim_Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dim_Periods",
                columns: table => new
                {
                    PeriodId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountingDate = table.Column<DateTime>(nullable: false),
                    isLocked = table.Column<bool>(nullable: false),
                    FundId = table.Column<int>(nullable: false),
                    AccountingPeriodsDIMPeriodId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_Periods", x => x.PeriodId);
                    table.ForeignKey(
                        name: "FK_dim_Periods_dim_Periods_AccountingPeriodsDIMPeriodId",
                        column: x => x.AccountingPeriodsDIMPeriodId,
                        principalTable: "dim_Periods",
                        principalColumn: "PeriodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dim_Periods_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "FundId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fact_InvestorHoldings",
                columns: table => new
                {
                    HoldingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Units = table.Column<decimal>(nullable: false),
                    AverageCost = table.Column<decimal>(nullable: false),
                    HoldingDate = table.Column<DateTime>(nullable: false),
                    HighWaterMark = table.Column<decimal>(nullable: true),
                    NetValuation = table.Column<decimal>(nullable: false),
                    ManagementFeesAccrued = table.Column<decimal>(nullable: false),
                    PerformanceFeesAccrued = table.Column<decimal>(nullable: false),
                    InvestorId = table.Column<int>(nullable: false),
                    FundId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fact_InvestorHoldings", x => x.HoldingId);
                    table.ForeignKey(
                        name: "FK_fact_InvestorHoldings_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "FundId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_InvestorHoldings_Investors_InvestorId",
                        column: x => x.InvestorId,
                        principalTable: "Investors",
                        principalColumn: "InvestorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FundInvestor",
                columns: table => new
                {
                    FundInvestorId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HighWaterMark = table.Column<decimal>(nullable: true),
                    InceptionDate = table.Column<DateTime>(nullable: false),
                    InvestorId = table.Column<int>(nullable: false),
                    FundId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundInvestor", x => x.FundInvestorId);
                    table.ForeignKey(
                        name: "FK_FundInvestor_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "FundId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FundInvestor_Investors_InvestorId",
                        column: x => x.InvestorId,
                        principalTable: "Investors",
                        principalColumn: "InvestorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bo_Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TradeAmount = table.Column<decimal>(nullable: false),
                    TradeDate = table.Column<DateTime>(nullable: false),
                    SettleDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    Fees = table.Column<decimal>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    isLocked = table.Column<bool>(nullable: false),
                    isCashTransaction = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    SecurityId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CustodianId = table.Column<int>(nullable: false),
                    FundId = table.Column<int>(nullable: false),
                    TransactionTypeId = table.Column<int>(nullable: false),
                    LinkedTradeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bo_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_bo_Transactions_dim_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "dim_Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bo_Transactions_dim_Custodians_CustodianId",
                        column: x => x.CustodianId,
                        principalTable: "dim_Custodians",
                        principalColumn: "CustodianId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bo_Transactions_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "FundId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bo_Transactions_bo_LinkedTrades_LinkedTradeId",
                        column: x => x.LinkedTradeId,
                        principalTable: "bo_LinkedTrades",
                        principalColumn: "LinkedTradeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bo_Transactions_dim_Securities_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "dim_Securities",
                        principalColumn: "SecurityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bo_Transactions_dim_TransactionType_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "dim_TransactionType",
                        principalColumn: "TransactionTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fact_Positions",
                columns: table => new
                {
                    PositionId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PositionDate = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    AverageCost = table.Column<decimal>(nullable: false),
                    MarketValue = table.Column<decimal>(nullable: false),
                    RealisedPnl = table.Column<decimal>(nullable: false),
                    UnrealisedPnl = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    FundId = table.Column<int>(nullable: false),
                    AssetClassId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    SecurityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fact_Positions", x => x.PositionId);
                    table.ForeignKey(
                        name: "FK_fact_Positions_dim_AssetClasses_AssetClassId",
                        column: x => x.AssetClassId,
                        principalTable: "dim_AssetClasses",
                        principalColumn: "AssetClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_Positions_dim_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "dim_Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_Positions_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "FundId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_Positions_dim_Securities_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "dim_Securities",
                        principalColumn: "SecurityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityPrices",
                columns: table => new
                {
                    PriceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    ClosePrice = table.Column<decimal>(nullable: false),
                    PriceSource = table.Column<string>(nullable: false),
                    SecurityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityPrices", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_SecurityPrices_dim_Securities_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "dim_Securities",
                        principalColumn: "SecurityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fact_NAVPrices",
                columns: table => new
                {
                    NAVPriceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FinalisedDate = table.Column<DateTime>(nullable: false),
                    NAVPrice = table.Column<decimal>(nullable: false),
                    SharesOutstanding = table.Column<decimal>(nullable: false),
                    NetAssetValue = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    FundId = table.Column<int>(nullable: false),
                    NAVPeriodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fact_NAVPrices", x => x.NAVPriceId);
                    table.ForeignKey(
                        name: "FK_fact_NAVPrices_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "FundId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_NAVPrices_dim_Periods_NAVPeriodId",
                        column: x => x.NAVPeriodId,
                        principalTable: "dim_Periods",
                        principalColumn: "PeriodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bo_TransferAgency",
                columns: table => new
                {
                    TransferAgencyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsNavFinal = table.Column<bool>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionSettleDate = table.Column<DateTime>(nullable: false),
                    Units = table.Column<decimal>(nullable: false),
                    NAVPrice = table.Column<decimal>(nullable: false),
                    TradeAmount = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    Fees = table.Column<decimal>(nullable: false),
                    IssueType = table.Column<string>(nullable: false),
                    FundId = table.Column<int>(nullable: false),
                    FundInvestorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bo_TransferAgency", x => x.TransferAgencyId);
                    table.ForeignKey(
                        name: "FK_bo_TransferAgency_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "FundId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bo_TransferAgency_FundInvestor_FundInvestorId",
                        column: x => x.FundInvestorId,
                        principalTable: "FundInvestor",
                        principalColumn: "FundInvestorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationSettings",
                columns: new[] { "SettingId", "Description", "SettingName", "SettingValue" },
                values: new object[] { 1, "Alpha Vantage API Key", "AlphaVantageAPI", "demo" });

            migrationBuilder.InsertData(
                table: "ApplicationSettings",
                columns: new[] { "SettingId", "Description", "SettingName", "SettingValue" },
                values: new object[] { 2, "Financial Modelling Prep API Key", "FMPrepAPI", "demo" });

            migrationBuilder.InsertData(
                table: "dim_AssetClasses",
                columns: new[] { "AssetClassId", "Name" },
                values: new object[] { 1, "Equity" });

            migrationBuilder.InsertData(
                table: "dim_AssetClasses",
                columns: new[] { "AssetClassId", "Name" },
                values: new object[] { 2, "Cryptocurrency" });

            migrationBuilder.InsertData(
                table: "dim_AssetClasses",
                columns: new[] { "AssetClassId", "Name" },
                values: new object[] { 3, "FX" });

            migrationBuilder.InsertData(
                table: "dim_AssetClasses",
                columns: new[] { "AssetClassId", "Name" },
                values: new object[] { 4, "Cash" });

            migrationBuilder.InsertData(
                table: "dim_AssetClasses",
                columns: new[] { "AssetClassId", "Name" },
                values: new object[] { 5, "InterestRate" });

            migrationBuilder.InsertData(
                table: "dim_AssetClasses",
                columns: new[] { "AssetClassId", "Name" },
                values: new object[] { 6, "FXForward" });

            migrationBuilder.InsertData(
                table: "dim_Currencies",
                columns: new[] { "CurrencyId", "Name", "Symbol" },
                values: new object[] { 8, "AustralianDollar", "AUD" });

            migrationBuilder.InsertData(
                table: "dim_Currencies",
                columns: new[] { "CurrencyId", "Name", "Symbol" },
                values: new object[] { 7, "CanadianDollar", "CAD" });

            migrationBuilder.InsertData(
                table: "dim_Currencies",
                columns: new[] { "CurrencyId", "Name", "Symbol" },
                values: new object[] { 6, "SwissFranc", "CHF" });

            migrationBuilder.InsertData(
                table: "dim_Currencies",
                columns: new[] { "CurrencyId", "Name", "Symbol" },
                values: new object[] { 5, "IndianRupee", "INR" });

            migrationBuilder.InsertData(
                table: "dim_Currencies",
                columns: new[] { "CurrencyId", "Name", "Symbol" },
                values: new object[] { 1, "PoundSterling", "GBP" });

            migrationBuilder.InsertData(
                table: "dim_Currencies",
                columns: new[] { "CurrencyId", "Name", "Symbol" },
                values: new object[] { 3, "UnitedStatesDollar", "USD" });

            migrationBuilder.InsertData(
                table: "dim_Currencies",
                columns: new[] { "CurrencyId", "Name", "Symbol" },
                values: new object[] { 2, "Euro", "EUR" });

            migrationBuilder.InsertData(
                table: "dim_Currencies",
                columns: new[] { "CurrencyId", "Name", "Symbol" },
                values: new object[] { 4, "JapaneseYen", "JPY" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 1, "Default", "Default" });

            migrationBuilder.InsertData(
                table: "dim_IssueTypes",
                columns: new[] { "IssueTypeID", "TypeName" },
                values: new object[] { 2, "Redemption" });

            migrationBuilder.InsertData(
                table: "dim_IssueTypes",
                columns: new[] { "IssueTypeID", "TypeName" },
                values: new object[] { 1, "Subscription" });

            migrationBuilder.InsertData(
                table: "dim_NavFrequencies",
                columns: new[] { "NavFrequencyId", "Frequency" },
                values: new object[] { 1, "Daily" });

            migrationBuilder.InsertData(
                table: "dim_NavFrequencies",
                columns: new[] { "NavFrequencyId", "Frequency" },
                values: new object[] { 2, "Monthly" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 1, "None", "SecurityTrade", "Trade" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 2, "None", "SecurityTrade", "Dividends" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 3, "Inflow", "CashTrade", "Income" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 4, "Outflow", "CashTrade", "Expense" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 5, "Inflow", "CashTrade", "Deposit" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 6, "Outflow", "CashTrade", "Withdrawal" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 7, "None", "CashTrade", "Interest" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 8, "Outflow", "CashTrade", "ManagementFee" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 9, "Outflow", "CashTrade", "PerformanceFee" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 10, "None", "CashTrade", "Miscellaneous" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 11, "Inflow", "CashTrade", "FXBuy" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 12, "Outflow", "CashTrade", "FXSell" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 13, "None", "FXTrade", "FXTrade" });

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 14, "None", "FXTrade", "FXTradeCollapse" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 1, null, 4, 1, null, null, "CASH GBP", "GBPc" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 9, null, 5, 1, null, null, "BOE Base Rate", "GBP_IRBASE" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 2, null, 4, 2, null, null, "CASH EURO", "EURc" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 10, null, 5, 2, null, null, "ECB Main Refinancing Rate", "EUR_IRBASE" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 3, null, 4, 3, null, null, "CASH USD", "USDc" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 11, null, 5, 3, null, null, "Federal Funds Rate", "USD_IRBASE" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 4, null, 4, 4, null, null, "CASH JPY", "JPYc" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 12, null, 5, 4, null, null, "BOJ Mutan Rate", "JPY_IRBASE" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 5, null, 4, 5, null, null, "CASH INR", "INRc" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 13, null, 5, 5, null, null, "BOI Repurchase Rate", "INR_IRBASE" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 6, null, 4, 6, null, null, "CASH CHF", "CHFc" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 14, null, 5, 6, null, null, "SNB Policy Rate", "CHF_IRBASE" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 7, null, 4, 7, null, null, "CASH CAD", "CADc" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 15, null, 5, 7, null, null, "BOC Policy Rate", "CAD_IRBASE" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 8, null, 4, 8, null, null, "CASH AUD", "AUDc" });

            migrationBuilder.InsertData(
                table: "dim_Securities",
                columns: new[] { "SecurityId", "AlphaVantageSymbol", "AssetClassId", "CurrencyId", "FMPSymbol", "ISIN", "SecurityName", "Symbol" },
                values: new object[] { 16, null, 5, 8, null, null, "RBA Cash Rate Target", "AUD_IRBASE" });

            migrationBuilder.CreateIndex(
                name: "IX_bo_Transactions_CurrencyId",
                table: "bo_Transactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_bo_Transactions_CustodianId",
                table: "bo_Transactions",
                column: "CustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_bo_Transactions_FundId",
                table: "bo_Transactions",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_bo_Transactions_LinkedTradeId",
                table: "bo_Transactions",
                column: "LinkedTradeId");

            migrationBuilder.CreateIndex(
                name: "IX_bo_Transactions_SecurityId",
                table: "bo_Transactions",
                column: "SecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_bo_Transactions_TransactionTypeId",
                table: "bo_Transactions",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_bo_TransferAgency_FundId",
                table: "bo_TransferAgency",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_bo_TransferAgency_FundInvestorId",
                table: "bo_TransferAgency",
                column: "FundInvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_dim_Periods_AccountingPeriodsDIMPeriodId",
                table: "dim_Periods",
                column: "AccountingPeriodsDIMPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_dim_Periods_FundId",
                table: "dim_Periods",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_dim_Securities_AssetClassId",
                table: "dim_Securities",
                column: "AssetClassId");

            migrationBuilder.CreateIndex(
                name: "IX_dim_Securities_CurrencyId",
                table: "dim_Securities",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_fact_InvestorHoldings_FundId",
                table: "fact_InvestorHoldings",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_fact_InvestorHoldings_InvestorId",
                table: "fact_InvestorHoldings",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_fact_NAVPrices_FundId",
                table: "fact_NAVPrices",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_fact_NAVPrices_NAVPeriodId",
                table: "fact_NAVPrices",
                column: "NAVPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_fact_Positions_AssetClassId",
                table: "fact_Positions",
                column: "AssetClassId");

            migrationBuilder.CreateIndex(
                name: "IX_fact_Positions_CurrencyId",
                table: "fact_Positions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_fact_Positions_FundId",
                table: "fact_Positions",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_fact_Positions_SecurityId",
                table: "fact_Positions",
                column: "SecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_FundInvestor_FundId",
                table: "FundInvestor",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_FundInvestor_InvestorId",
                table: "FundInvestor",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityPrices_SecurityId",
                table: "SecurityPrices",
                column: "SecurityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSettings");

            migrationBuilder.DropTable(
                name: "bo_Transactions");

            migrationBuilder.DropTable(
                name: "bo_TransferAgency");

            migrationBuilder.DropTable(
                name: "dim_IssueTypes");

            migrationBuilder.DropTable(
                name: "dim_NavFrequencies");

            migrationBuilder.DropTable(
                name: "fact_FundPerformance");

            migrationBuilder.DropTable(
                name: "fact_InvestorHoldings");

            migrationBuilder.DropTable(
                name: "fact_NAVPrices");

            migrationBuilder.DropTable(
                name: "fact_Positions");

            migrationBuilder.DropTable(
                name: "SecurityPrices");

            migrationBuilder.DropTable(
                name: "dim_Custodians");

            migrationBuilder.DropTable(
                name: "bo_LinkedTrades");

            migrationBuilder.DropTable(
                name: "dim_TransactionType");

            migrationBuilder.DropTable(
                name: "FundInvestor");

            migrationBuilder.DropTable(
                name: "dim_Periods");

            migrationBuilder.DropTable(
                name: "dim_Securities");

            migrationBuilder.DropTable(
                name: "Investors");

            migrationBuilder.DropTable(
                name: "Fund");

            migrationBuilder.DropTable(
                name: "dim_AssetClasses");

            migrationBuilder.DropTable(
                name: "dim_Currencies");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioAce.EFCore.Migrations
{
    public partial class allStartupData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 1,
                columns: new[] { "Name", "Symbol" },
                values: new object[] { "eToro", "ETOR" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 20, "Default", "Default" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 19, "CoinBase", "COIN" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 18, "Barclays", "BARC" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 17, "tastyworks", "TASTY" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 16, "TransferWise", "TWISE" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 14, "TD Ameritrade", "TDAM" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 13, "E*TRADE", "ETRAD" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 12, "Fidelity", "FIDEL" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 15, "Interactive Brokers", "IBKR" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 10, "Interactive Investor", "III" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 11, "RobinHood", "ROBH" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 4, "City Index", "CIDEX" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 5, "Plus500", "P500" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 6, "IG", "IG" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 3, "Halifax", "HFSD" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 8, "Degiro", "DEGO" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 9, "Hargreaves Lansdown", "HGLN" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 7, "Revolut", "REVO" });

            migrationBuilder.InsertData(
                table: "dim_Custodians",
                columns: new[] { "CustodianId", "Name", "Symbol" },
                values: new object[] { 2, "Trading212", "T212" });

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 5,
                column: "TypeClass",
                value: "CapitalTrade");

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 6,
                column: "TypeClass",
                value: "CapitalTrade");

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 10,
                columns: new[] { "Direction", "TypeName" },
                values: new object[] { "Outflow", "ManagementFee" });

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 11,
                column: "TypeName",
                value: "CashTransfer");

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 12,
                columns: new[] { "Direction", "TypeName" },
                values: new object[] { "Inflow", "FXBuy" });

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 13,
                columns: new[] { "Direction", "TypeClass", "TypeName" },
                values: new object[] { "Outflow", "CashTrade", "FXSell" });

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 14,
                column: "TypeName",
                value: "FXTrade");

            migrationBuilder.InsertData(
                table: "dim_TransactionType",
                columns: new[] { "TransactionTypeId", "Direction", "TypeClass", "TypeName" },
                values: new object[] { 15, "None", "FXTrade", "FXTradeCollapse" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "dim_Custodians",
                keyColumn: "CustodianId",
                keyValue: 1,
                columns: new[] { "Name", "Symbol" },
                values: new object[] { "Default", "Default" });

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 5,
                column: "TypeClass",
                value: "CashTrade");

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 6,
                column: "TypeClass",
                value: "CashTrade");

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 10,
                columns: new[] { "Direction", "TypeName" },
                values: new object[] { "None", "Miscellaneous" });

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 11,
                column: "TypeName",
                value: "FXBuy");

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 12,
                columns: new[] { "Direction", "TypeName" },
                values: new object[] { "Outflow", "FXSell" });

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 13,
                columns: new[] { "Direction", "TypeClass", "TypeName" },
                values: new object[] { "None", "FXTrade", "FXTrade" });

            migrationBuilder.UpdateData(
                table: "dim_TransactionType",
                keyColumn: "TransactionTypeId",
                keyValue: 14,
                column: "TypeName",
                value: "FXTradeCollapse");
        }
    }
}

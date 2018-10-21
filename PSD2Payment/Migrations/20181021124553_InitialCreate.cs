using Microsoft.EntityFrameworkCore.Migrations;

namespace PSD2Payment.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "psd2");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "psd2",
                columns: table => new
                {
                    ResourceId = table.Column<string>(nullable: true),
                    Iban = table.Column<string>(nullable: false),
                    Bban = table.Column<string>(nullable: true),
                    Msisdn = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Product = table.Column<string>(nullable: true),
                    CashAccountType = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Bic = table.Column<string>(nullable: true),
                    LinkedAccounts = table.Column<string>(nullable: true),
                    Usage = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Links = table.Column<string>(nullable: true),
                    Balances = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Iban);
                });

            migrationBuilder.InsertData(
                schema: "psd2",
                table: "Accounts",
                columns: new[] { "Iban", "Bban", "Bic", "CashAccountType", "Currency", "Details", "LinkedAccounts", "Links", "Msisdn", "Name", "Product", "ResourceId", "Status", "Usage", "Balances" },
                values: new object[] { "RS35105008123123123173", null, null, "multi-currency", "RSD", null, null, "{\"balances\":null,\"transactions\":null}", null, "Multi-currency Current account", "current-account", "159c9e96-2247-4bdf-864a-8704f708861a", "active", null, "[{\"balanceAmount\":{\"currency\":\"EUR\",\"amount\":10000.0},\"balanceType\":\"current\",\"lastChangeDateTime\":\"2018-10-21T14:45:53.5153003+02:00\",\"referenceDate\":null,\"lastCommittedTransaction\":null},{\"balanceAmount\":{\"currency\":\"RSD\",\"amount\":128000.0},\"balanceType\":\"current\",\"lastChangeDateTime\":\"2018-10-21T14:45:53.5209942+02:00\",\"referenceDate\":null,\"lastCommittedTransaction\":null}]" });

            migrationBuilder.InsertData(
                schema: "psd2",
                table: "Accounts",
                columns: new[] { "Iban", "Bban", "Bic", "CashAccountType", "Currency", "Details", "LinkedAccounts", "Links", "Msisdn", "Name", "Product", "ResourceId", "Status", "Usage", "Balances" },
                values: new object[] { "RS3510523423123123123173", null, null, "multi-currency", "EUR", null, null, "{\"balances\":null,\"transactions\":null}", null, "Multi-currency Current account", "current-account", "c6122c5d-a990-4234-86a4-6894775c1c79", "active", null, "[{\"balanceAmount\":{\"currency\":\"EUR\",\"amount\":6500.0},\"balanceType\":\"current\",\"lastChangeDateTime\":\"2018-10-21T14:45:53.5213122+02:00\",\"referenceDate\":null,\"lastCommittedTransaction\":null},{\"balanceAmount\":{\"currency\":\"RSD\",\"amount\":765000.0},\"balanceType\":\"current\",\"lastChangeDateTime\":\"2018-10-21T14:45:53.5213137+02:00\",\"referenceDate\":null,\"lastCommittedTransaction\":null}]" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "psd2");
        }
    }
}

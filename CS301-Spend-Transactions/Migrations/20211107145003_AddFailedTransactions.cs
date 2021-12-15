using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CS301_Spend_Transactions.Migrations
{
    public partial class AddFailedTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "failed_transactions",
                columns: table => new
                {
                    transaction_id = table.Column<string>(nullable: false),
                    id = table.Column<string>(nullable: true),
                    mercant = table.Column<string>(nullable: true),
                    mcc = table.Column<int>(nullable: true),
                    currency = table.Column<string>(nullable: true),
                    amount = table.Column<decimal>(nullable: false),
                    transaction_date = table.Column<DateTime>(nullable: false),
                    card_id = table.Column<string>(nullable: true),
                    card_pan = table.Column<string>(nullable: true),
                    card_type = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("failed_transactions_pkey", x => x.transaction_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "failed_transactions");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace CS301_Spend_Transactions.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exclusions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    card_type = table.Column<string>(nullable: true),
                    mcc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("exclusions_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    min_mcc = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    max_mcc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("groups_pkey", x => x.min_mcc);
                });

            migrationBuilder.CreateTable(
                name: "merchants",
                columns: table => new
                {
                    name = table.Column<string>(nullable: false),
                    mcc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("merchants_pkey", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "points_type",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(nullable: true),
                    unit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rewards_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    first_name = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    phone_no = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    card_type = table.Column<string>(nullable: true),
                    min_spend = table.Column<decimal>(nullable: false),
                    max_spend = table.Column<decimal>(nullable: false),
                    foreign_spend = table.Column<bool>(nullable: false),
                    multiplier = table.Column<decimal>(nullable: false),
                    PointsTypeId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    start_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true),
                    MerchantName = table.Column<string>(nullable: true),
                    MCC = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rules_pkey", x => x.id);
                    table.ForeignKey(
                        name: "campaign_merchant_fkey",
                        column: x => x.MerchantName,
                        principalTable: "merchants",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "pointsType_rule_fkey",
                        column: x => x.PointsTypeId,
                        principalTable: "points_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    card_pan = table.Column<string>(nullable: true),
                    card_type = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cards_pkey", x => x.id);
                    table.ForeignKey(
                        name: "card_user_fkey",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    transaction_date = table.Column<DateTime>(nullable: false),
                    currency = table.Column<string>(nullable: true),
                    amount = table.Column<decimal>(nullable: false),
                    CardId = table.Column<string>(nullable: true),
                    MerchantName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transactions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "transaction_card_fkey",
                        column: x => x.CardId,
                        principalTable: "cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "transaction_merchant_fkey",
                        column: x => x.MerchantName,
                        principalTable: "merchants",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "points",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    amount = table.Column<decimal>(nullable: false),
                    processed_date = table.Column<DateTime>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true),
                    PointsTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("points_pkey", x => x.id);
                    table.ForeignKey(
                        name: "pointsType_point_fkey",
                        column: x => x.PointsTypeId,
                        principalTable: "points_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "point_transaction_fkey",
                        column: x => x.TransactionId,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cards_UserId",
                table: "cards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_points_PointsTypeId",
                table: "points",
                column: "PointsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_points_TransactionId",
                table: "points",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_rules_MerchantName",
                table: "rules",
                column: "MerchantName");

            migrationBuilder.CreateIndex(
                name: "IX_rules_PointsTypeId",
                table: "rules",
                column: "PointsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_CardId",
                table: "transactions",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_MerchantName",
                table: "transactions",
                column: "MerchantName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exclusions");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "points");

            migrationBuilder.DropTable(
                name: "rules");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "points_type");

            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "merchants");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

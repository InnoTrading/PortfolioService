using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStockEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStocks_Stocks_StockID",
                table: "UserStocks");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_UserStocks_StockID",
                table: "UserStocks");

            migrationBuilder.DropColumn(
                name: "StockID",
                table: "UserStocks");

            migrationBuilder.AddColumn<string>(
                name: "StockTicker",
                table: "UserStocks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockTicker",
                table: "UserStocks");

            migrationBuilder.AddColumn<Guid>(
                name: "StockID",
                table: "UserStocks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Ticker = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStocks_StockID",
                table: "UserStocks",
                column: "StockID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStocks_Stocks_StockID",
                table: "UserStocks",
                column: "StockID",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

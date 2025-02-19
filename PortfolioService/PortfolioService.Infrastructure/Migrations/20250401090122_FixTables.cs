using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntity_BaseEntity_StockID",
                table: "BaseEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntity_BaseEntity_UserID",
                table: "BaseEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntity_BaseEntity_UserId",
                table: "BaseEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseEntity",
                table: "BaseEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntity_UserId",
                table: "BaseEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntity_UserID",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Auth0Id",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "ReservedBalance",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Ticker",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "UserEntity_Auth0Id",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "BaseEntity");

            migrationBuilder.RenameTable(
                name: "BaseEntity",
                newName: "UserStocks");

            migrationBuilder.RenameIndex(
                name: "IX_BaseEntity_StockID",
                table: "UserStocks",
                newName: "IX_UserStocks_StockID");

            migrationBuilder.AlterColumn<Guid>(
                name: "StockID",
                table: "UserStocks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "UserStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Auth0UserID",
                table: "UserStocks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStocks",
                table: "UserStocks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Auth0UserId = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    ReservedBalance = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Ticker = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserStocks_Stocks_StockID",
                table: "UserStocks",
                column: "StockID",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStocks_Stocks_StockID",
                table: "UserStocks");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStocks",
                table: "UserStocks");

            migrationBuilder.DropColumn(
                name: "Auth0UserID",
                table: "UserStocks");

            migrationBuilder.RenameTable(
                name: "UserStocks",
                newName: "BaseEntity");

            migrationBuilder.RenameIndex(
                name: "IX_UserStocks_StockID",
                table: "BaseEntity",
                newName: "IX_BaseEntity_StockID");

            migrationBuilder.AlterColumn<Guid>(
                name: "StockID",
                table: "BaseEntity",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "BaseEntity",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Auth0Id",
                table: "BaseEntity",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "BaseEntity",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BaseEntity",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BaseEntity",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BaseEntity",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BaseEntity",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReservedBalance",
                table: "BaseEntity",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ticker",
                table: "BaseEntity",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "BaseEntity",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEntity_Auth0Id",
                table: "BaseEntity",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "BaseEntity",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BaseEntity",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "BaseEntity",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseEntity",
                table: "BaseEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_UserId",
                table: "BaseEntity",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_UserID",
                table: "BaseEntity",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntity_BaseEntity_StockID",
                table: "BaseEntity",
                column: "StockID",
                principalTable: "BaseEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntity_BaseEntity_UserID",
                table: "BaseEntity",
                column: "UserID",
                principalTable: "BaseEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntity_BaseEntity_UserId",
                table: "BaseEntity",
                column: "UserId",
                principalTable: "BaseEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusErp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSalesReturnUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturns_Users_UserId",
                table: "SalesReturns");

            migrationBuilder.DropIndex(
                name: "IX_SalesReturns_UserId",
                table: "SalesReturns");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SalesReturns");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_CreatedByUserId",
                table: "SalesReturns",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturns_Users_CreatedByUserId",
                table: "SalesReturns",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturns_Users_CreatedByUserId",
                table: "SalesReturns");

            migrationBuilder.DropIndex(
                name: "IX_SalesReturns_CreatedByUserId",
                table: "SalesReturns");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "SalesReturns",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_UserId",
                table: "SalesReturns",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturns_Users_UserId",
                table: "SalesReturns",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

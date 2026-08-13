using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusErp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixProductBatchAndAddRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ProductBatches",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "BranchStocks",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProductBatches");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "BranchStocks");
        }
    }
}

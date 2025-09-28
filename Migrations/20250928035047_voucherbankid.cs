using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FintcsApi.Migrations
{
    /// <inheritdoc />
    public partial class voucherbankid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_LedgerTransactions_LedgerTransactionId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_LedgerTransactionId",
                table: "Vouchers");

            migrationBuilder.AddColumn<decimal>(
                name: "BankId",
                table: "Vouchers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Vouchers");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_LedgerTransactionId",
                table: "Vouchers",
                column: "LedgerTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_LedgerTransactions_LedgerTransactionId",
                table: "Vouchers",
                column: "LedgerTransactionId",
                principalTable: "LedgerTransactions",
                principalColumn: "LedgerTransactionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FintcsApi.Migrations
{
    /// <inheritdoc />
    public partial class voucherupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoucherNumber",
                table: "Vouchers");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChecqueDate",
                table: "Vouchers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChecqueNumber",
                table: "Vouchers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParticularId",
                table: "Vouchers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayId",
                table: "Vouchers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SocietyId",
                table: "Vouchers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParticularId",
                table: "LedgerTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayId",
                table: "LedgerTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChecqueDate",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ChecqueNumber",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ParticularId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "PayId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "SocietyId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ParticularId",
                table: "LedgerTransactions");

            migrationBuilder.DropColumn(
                name: "PayId",
                table: "LedgerTransactions");

            migrationBuilder.AddColumn<string>(
                name: "VoucherNumber",
                table: "Vouchers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}

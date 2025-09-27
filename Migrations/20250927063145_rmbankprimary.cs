using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FintcsApi.Migrations
{
    /// <inheritdoc />
    public partial class rmbankprimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "SocietyBankAccounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "SocietyBankAccounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

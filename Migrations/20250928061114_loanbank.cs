using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FintcsApi.Migrations
{
    /// <inheritdoc />
    public partial class loanbank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Replace NULLs with 0
            migrationBuilder.Sql(@"
                UPDATE ""Loans"" 
                SET ""Bank"" = '0' 
                WHERE ""Bank"" IS NULL;
            ");

            // 2️⃣ Replace any non-numeric values with 0
            migrationBuilder.Sql(@"
                UPDATE ""Loans"" 
                SET ""Bank"" = '0' 
                WHERE NOT ""Bank"" ~ '^\d+$';
            ");

            // 3️⃣ Alter column type safely using explicit cast
            migrationBuilder.Sql(@"
                ALTER TABLE ""Loans""
                ALTER COLUMN ""Bank"" TYPE integer
                USING ""Bank""::integer;
            ");

            // 4️⃣ Set NOT NULL and default
            migrationBuilder.AlterColumn<int>(
                name: "Bank",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert column back to string (text)
            migrationBuilder.AlterColumn<string>(
                name: "Bank",
                table: "Loans",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}

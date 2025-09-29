using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FintcsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDemandTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Demands",
                columns: table => new
                {
                    DemandId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MemberId = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    CD = table.Column<int>(type: "integer", nullable: false),
                    OD = table.Column<int>(type: "integer", nullable: false),
                    Share = table.Column<int>(type: "integer", nullable: false),
                    NetLoanAmount = table.Column<int>(type: "integer", nullable: false),
                    BuildingFund = table.Column<int>(type: "integer", nullable: false),
                    PenalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PenalInterest = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demands", x => x.DemandId);
                    table.ForeignKey(
                        name: "FK_Demands_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemandLoan",
                columns: table => new
                {
                    DemandLoanId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DemandId = table.Column<int>(type: "integer", nullable: false),
                    LoanType = table.Column<string>(type: "text", nullable: false),
                    PendingAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Installment = table.Column<decimal>(type: "numeric", nullable: false),
                    Interest = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandLoan", x => x.DemandLoanId);
                    table.ForeignKey(
                        name: "FK_DemandLoan_Demands_DemandId",
                        column: x => x.DemandId,
                        principalTable: "Demands",
                        principalColumn: "DemandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandLoan_DemandId",
                table: "DemandLoan",
                column: "DemandId");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_MemberId",
                table: "Demands",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandLoan");

            migrationBuilder.DropTable(
                name: "Demands");
        }
    }
}

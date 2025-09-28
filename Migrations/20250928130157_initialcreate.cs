using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FintcsApi.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Societies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ChequeBounceCharge = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Societies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressOffice = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AddressResidential = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Designation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PhoneOffice = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PhoneResidential = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanTypes",
                columns: table => new
                {
                    LoanTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SocietyId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    InterestPercent = table.Column<decimal>(type: "numeric", nullable: false),
                    LimitAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CompulsoryDeposit = table.Column<decimal>(type: "numeric", nullable: false),
                    OptionalDeposit = table.Column<decimal>(type: "numeric", nullable: false),
                    ShareAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    XTimes = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanTypes", x => x.LoanTypeId);
                    table.ForeignKey(
                        name: "FK_LoanTypes_Societies_SocietyId",
                        column: x => x.SocietyId,
                        principalTable: "Societies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FHName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OfficeAddress = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    PhoneOffice = table.Column<string>(type: "text", nullable: false),
                    Branch = table.Column<string>(type: "text", nullable: false),
                    PhoneRes = table.Column<string>(type: "text", nullable: false),
                    Designation = table.Column<string>(type: "text", nullable: false),
                    ResidenceAddress = table.Column<string>(type: "text", nullable: false),
                    DOB = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DOJSociety = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DOR = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Nominee = table.Column<string>(type: "text", nullable: false),
                    NomineeRelation = table.Column<string>(type: "text", nullable: false),
                    CdAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Email2 = table.Column<string>(type: "text", nullable: false),
                    Mobile2 = table.Column<string>(type: "text", nullable: false),
                    Pincode = table.Column<string>(type: "text", nullable: false),
                    BankName = table.Column<string>(type: "text", nullable: false),
                    AccountNumber = table.Column<string>(type: "text", nullable: false),
                    PayableAt = table.Column<string>(type: "text", nullable: false),
                    Share = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SocietyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Societies_SocietyId",
                        column: x => x.SocietyId,
                        principalTable: "Societies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocietyBankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SocietyId = table.Column<int>(type: "integer", nullable: false),
                    BankName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IFSC = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Branch = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocietyBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocietyBankAccounts_Societies_SocietyId",
                        column: x => x.SocietyId,
                        principalTable: "Societies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LedgerAccounts",
                columns: table => new
                {
                    LedgerAccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MemberId = table.Column<int>(type: "integer", nullable: true),
                    AccountName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerAccounts", x => x.LedgerAccountId);
                    table.ForeignKey(
                        name: "FK_LedgerAccounts_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    LoanId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SocietyId = table.Column<int>(type: "integer", nullable: false),
                    MemberId = table.Column<int>(type: "integer", nullable: false),
                    LoanTypeId = table.Column<int>(type: "integer", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PreviousLoan = table.Column<decimal>(type: "numeric", nullable: false),
                    Installments = table.Column<int>(type: "integer", nullable: false),
                    Purpose = table.Column<string>(type: "text", nullable: true),
                    AuthorizedBy = table.Column<string>(type: "text", nullable: true),
                    PaymentMode = table.Column<string>(type: "text", nullable: false),
                    Bank = table.Column<int>(type: "integer", nullable: false),
                    ChequeNo = table.Column<string>(type: "text", nullable: true),
                    ChequeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    NetLoan = table.Column<decimal>(type: "numeric", nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    NewLoanShare = table.Column<decimal>(type: "numeric", nullable: false),
                    PayAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.LoanId);
                    table.ForeignKey(
                        name: "FK_Loans_LoanTypes_LoanTypeId",
                        column: x => x.LoanTypeId,
                        principalTable: "LoanTypes",
                        principalColumn: "LoanTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Societies_SocietyId",
                        column: x => x.SocietyId,
                        principalTable: "Societies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    VoucherId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PayId = table.Column<int>(type: "integer", nullable: false),
                    ParticularId = table.Column<int>(type: "integer", nullable: false),
                    SocietyId = table.Column<int>(type: "integer", nullable: false),
                    VoucherType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VoucherDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Narration = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MemberId = table.Column<int>(type: "integer", nullable: true),
                    LoanId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BankId = table.Column<decimal>(type: "numeric", nullable: false),
                    ChecqueNumber = table.Column<string>(type: "text", nullable: true),
                    ChecqueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.VoucherId);
                    table.ForeignKey(
                        name: "FK_Vouchers_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "LoanId");
                    table.ForeignKey(
                        name: "FK_Vouchers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LedgerTransactions",
                columns: table => new
                {
                    LedgerTransactionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LedgerAccountId = table.Column<int>(type: "integer", nullable: false),
                    ParticularId = table.Column<int>(type: "integer", nullable: false),
                    PayId = table.Column<int>(type: "integer", nullable: false),
                    MemberId = table.Column<int>(type: "integer", nullable: true),
                    LoanId = table.Column<int>(type: "integer", nullable: true),
                    SocietyId = table.Column<int>(type: "integer", nullable: false),
                    BankId = table.Column<int>(type: "integer", nullable: true),
                    Debit = table.Column<decimal>(type: "numeric", nullable: false),
                    Credit = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VoucherId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerTransactions", x => x.LedgerTransactionId);
                    table.ForeignKey(
                        name: "FK_LedgerTransactions_LedgerAccounts_LedgerAccountId",
                        column: x => x.LedgerAccountId,
                        principalTable: "LedgerAccounts",
                        principalColumn: "LedgerAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LedgerTransactions_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "VoucherId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccounts_MemberId",
                table: "LedgerAccounts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerTransactions_LedgerAccountId",
                table: "LedgerTransactions",
                column: "LedgerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerTransactions_VoucherId",
                table: "LedgerTransactions",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanTypeId",
                table: "Loans",
                column: "LoanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MemberId",
                table: "Loans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_SocietyId",
                table: "Loans",
                column: "SocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanTypes_SocietyId_Name",
                table: "LoanTypes",
                columns: new[] { "SocietyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_SocietyId",
                table: "Members",
                column: "SocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_Societies_Address",
                table: "Societies",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Societies_Email",
                table: "Societies",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Societies_Fax",
                table: "Societies",
                column: "Fax",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Societies_Name",
                table: "Societies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Societies_Phone",
                table: "Societies",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Societies_RegistrationNumber",
                table: "Societies",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Societies_Website",
                table: "Societies",
                column: "Website",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocietyBankAccounts_SocietyId_BankName_AccountNumber",
                table: "SocietyBankAccounts",
                columns: new[] { "SocietyId", "BankName", "AccountNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_LoanId",
                table: "Vouchers",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_MemberId",
                table: "Vouchers",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LedgerTransactions");

            migrationBuilder.DropTable(
                name: "SocietyBankAccounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LedgerAccounts");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "LoanTypes");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Societies");
        }
    }
}

// Data/AppDbContext.cs

using FintcsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FintcsApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }   // <-- re-add this
        public DbSet<Society> Societies { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<SocietyBankAccount> SocietyBankAccounts { get; set; }
        public DbSet<Member> Members { get; set; } = default!;
        public DbSet<Loan> Loans { get; set; } = default!;
        public DbSet<LedgerAccount> LedgerAccounts { get; set; } = default!;
        public DbSet<LedgerTransaction> LedgerTransactions { get; set; } = default!;
        public DbSet<Voucher> Vouchers { get; set; } = default!;




        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User uniqueness
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Society uniqueness constraints
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Email).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Phone).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Fax).IsUnique();
            // modelBuilder.Entity<Society>()
            //     .HasIndex(s => s.Address).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Website).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.RegistrationNumber).IsUnique();

            // LoanType uniqueness per society
            modelBuilder.Entity<LoanType>()
                .HasIndex(lt => new { lt.SocietyId, lt.Name }).IsUnique();

            // BankAccount uniqueness per society
            modelBuilder.Entity<SocietyBankAccount>()
                .HasIndex(ba => new { ba.SocietyId, ba.BankName, ba.AccountNumber }).IsUnique();
        }
    }
}

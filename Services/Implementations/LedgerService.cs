using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FintcsApi.Services.Implementations
{
    public class LedgerService : ILedgerService
    {
        private readonly AppDbContext _context;

        public LedgerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LedgerAccount>> GetAllLedgerAccountsAsync()
        {
            return await _context.LedgerAccounts.ToListAsync();
        }

        public async Task<LedgerAccount?> GetLedgerAccountByIdAsync(int ledgerAccountId)
        {
            return await _context.LedgerAccounts
                .FirstOrDefaultAsync(l => l.LedgerAccountId == ledgerAccountId);
        }

        // Create default ledgers for a member
        public async Task CreateDefaultLedgersForMemberAsync(int memberId)
        {
            string[] defaultLedgers = new string[]
            {
                "Admission Fee Ledger",
                "Building Fund Ledger",
                "CD Ledger",
                "OD Ledger",
                "Share Ledger"
            };

            foreach (var ledgerName in defaultLedgers)
            {
                if (!await _context.LedgerAccounts.AnyAsync(l => l.MemberId == memberId && l.AccountName == ledgerName))
                {
                    _context.LedgerAccounts.Add(new LedgerAccount
                    {
                        MemberId = memberId,
                        AccountName = ledgerName,
                        Balance = 0
                    });
                }
            }

            // Add loan type ledgers dynamically
            var loanTypes = await _context.LoanTypes.ToListAsync();
            foreach (var loanType in loanTypes)
            {
                string loanLedgerName = $"{loanType.Name} Loan Ledger";
                if (!await _context.LedgerAccounts.AnyAsync(l => l.MemberId == memberId && l.AccountName == loanLedgerName))
                {
                    _context.LedgerAccounts.Add(new LedgerAccount
                    {
                        MemberId = memberId,
                        AccountName = loanLedgerName,
                        Balance = 0
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        // Record a ledger transaction
        public async Task RecordTransactionAsync(LedgerTransactionDto dto)
        {
            var ledger = await _context.LedgerAccounts.FindAsync(dto.LedgerAccountId);
            if (ledger == null) throw new Exception("Ledger account not found");

            ledger.Balance = ledger.Balance + dto.Credit - dto.Debit;
            _context.LedgerAccounts.Update(ledger);

            var transaction = new LedgerTransaction
            {
                LedgerAccountId = dto.LedgerAccountId,
                MemberId = dto.MemberId,
                LoanId = dto.LoanId,
                Debit = dto.Debit,
                Credit = dto.Credit,
                ParticularId = dto.ParticularId,
                PayId = dto.PayId,
                SocietyId = dto.SocietyId,
                BankId = dto.BankId,
                Description = dto.Description,
                TransactionDate = DateTime.UtcNow
            };

            _context.LedgerTransactions.Add(transaction);
            await _context.SaveChangesAsync(); // Transaction ID is int now
        }

        // Create other ledger
        public async Task CreateOtherLedgerAsync(int? memberId, string accountName, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentException("Account name is required");

            bool exists = await _context.LedgerAccounts
                .AnyAsync(l => l.MemberId == memberId && l.AccountName == accountName);

            if (exists)
                throw new Exception("Ledger account already exists");

            var ledger = new LedgerAccount
            {
                MemberId = memberId,
                AccountName = accountName,
                Balance = initialBalance
            };

            _context.LedgerAccounts.Add(ledger);
            await _context.SaveChangesAsync();
        }

        // Record transaction for other ledger
        public async Task RecordOtherLedgerTransactionAsync(LedgerTransactionDto dto)
        {
            var ledger = await _context.LedgerAccounts.FindAsync(dto.LedgerAccountId);
            if (ledger == null) throw new Exception("Ledger account not found");

            ledger.Balance = ledger.Balance + dto.Credit - dto.Debit;
            _context.LedgerAccounts.Update(ledger);

            var transaction = new LedgerTransaction
            {
                LedgerAccountId = dto.LedgerAccountId,
                MemberId = dto.MemberId,
                LoanId = dto.LoanId,
                Debit = dto.Debit,
                Credit = dto.Credit,
                ParticularId = dto.ParticularId,
                PayId = dto.PayId,
                SocietyId = dto.SocietyId,
                BankId = dto.BankId,
                Description = dto.Description,
                TransactionDate = DateTime.UtcNow
            };

            _context.LedgerTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
    }
}

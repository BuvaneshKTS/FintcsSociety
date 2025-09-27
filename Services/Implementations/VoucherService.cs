using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Services.Implementations
{
    public class VoucherService : IVoucherService
    {
        private readonly AppDbContext _context;

        public VoucherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Voucher> CreateVoucherAsync(CreateVoucherDto dto)
        {
            if (dto.Entries == null || !dto.Entries.Any())
                throw new ArgumentException("Voucher must have at least one entry.");

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create voucher
                var voucher = new Voucher
                {
                    VoucherType = dto.VoucherType,
                    MemberId = dto.MemberId,
                    LoanId = dto.LoanId,
                    Narration = dto.Narration,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync(); // VoucherId generated here

                // Fetch all relevant ledger accounts at once to optimize DB calls
                var ledgerIds = dto.Entries.Select(e => e.LedgerAccountId).ToList();
                var ledgers = await _context.LedgerAccounts
                    .Where(l => ledgerIds.Contains(l.LedgerAccountId))
                    .ToDictionaryAsync(l => l.LedgerAccountId);

                foreach (var entry in dto.Entries)
                {
                    if (!ledgers.TryGetValue(entry.LedgerAccountId, out var ledger))
                        throw new Exception($"Ledger account not found: {entry.LedgerAccountId}");

                    // Update ledger balance
                    ledger.Balance += entry.Credit - entry.Debit;
                    _context.LedgerAccounts.Update(ledger);

                    // Add ledger transaction
                    _context.LedgerTransactions.Add(new LedgerTransaction
                    {
                        VoucherId = voucher.VoucherId,
                        LedgerAccountId = entry.LedgerAccountId,
                        MemberId = dto.MemberId,
                        LoanId = dto.LoanId,
                        Debit = entry.Debit,
                        Credit = entry.Credit,
                        Description = entry.Description,
                        TransactionDate = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return voucher;
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                throw new Exception($"Voucher creation failed: {ex.Message}", ex);
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


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
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            int nextPayId = 1;
                var lastVoucher = await _context.Vouchers
                    .OrderByDescending(v => v.PayId)
                    .FirstOrDefaultAsync();
                if (lastVoucher != null)
                    nextPayId = lastVoucher.PayId + 1;

            // Create voucher
            var voucher = new Voucher
            {
                VoucherType = dto.VoucherType,
                MemberId = dto.MemberId,
                LoanId = dto.LoanId,
                Narration = dto.Narration,
                Amount = dto.Amount,
                BankId = dto.BankId,
                ChecqueNumber = dto.ChequeNumber,
                ChecqueDate = dto.ChequeDate,
                VoucherDate = dto.VoucherDate,
                PayId = nextPayId,
                ParticularId = dto.ParticularId,
                SocietyId = dto.SocietyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();

            var societyLedger = await _context.LedgerAccounts
                    .FirstOrDefaultAsync(la => la.AccountName == "Society Account");
            
            Console.WriteLine("Society Ledger :", societyLedger);
            
            var otherLedger = await _context.LedgerAccounts
                    .FirstOrDefaultAsync(la=> la.LedgerAccountId == dto.ParticularId);


            if(dto.VoucherType == "Receipt" && dto.LoanId != null){
                var transaction = new LedgerTransaction
                {
                    LedgerAccountId = dto.ParticularId,
                    MemberId = dto.MemberId,
                    LoanId = dto.LoanId,
                    Debit = dto.Amount,
                    Credit = 0,
                    ParticularId = dto.ParticularId,
                    PayId = nextPayId,
                    SocietyId = dto.SocietyId,
                    BankId = (int?)dto.BankId,
                    Description = dto.Narration,
                    VoucherId = voucher.VoucherId,
                    TransactionDate = DateTime.UtcNow
                };

                _context.LedgerTransactions.Add(transaction);
                await _context.SaveChangesAsync();

                otherLedger.Balance = (int)otherLedger.Balance - dto.Amount; // assuming Credit increases balance
                _context.LedgerAccounts.Update(otherLedger);
                await _context.SaveChangesAsync();



                var transaction1 = new LedgerTransaction
                {
                    LedgerAccountId = 1,
                    MemberId = dto.MemberId,
                    LoanId = dto.LoanId,
                    Debit = 0,
                    Credit = dto.Amount,
                    ParticularId = 1,
                    PayId = nextPayId,
                    SocietyId = dto.SocietyId,
                    BankId = (int)dto.BankId,
                    Description = dto.Narration,
                    VoucherId = voucher.VoucherId,
                    TransactionDate = DateTime.UtcNow
                };

                _context.LedgerTransactions.Add(transaction1);
                await _context.SaveChangesAsync();

                societyLedger.Balance = (int)societyLedger.Balance - dto.Amount;
                //  societyLedger.Balance += dto.LoanAmount; // assuming Credit increases balance
                _context.LedgerAccounts.Update( societyLedger);
                await _context.SaveChangesAsync();
            }
            else if(dto.VoucherType == "Receipt"){
                var transaction2 = new LedgerTransaction
                {
                    LedgerAccountId = dto.ParticularId,
                    MemberId = dto.MemberId,
                    LoanId = dto.LoanId,
                    Debit = dto.Amount,
                    Credit = 0,
                    ParticularId = dto.ParticularId,
                    PayId = nextPayId,
                    SocietyId = dto.SocietyId,
                    BankId = (int)dto.BankId,
                    Description = dto.Narration,
                    VoucherId = voucher.VoucherId,
                    TransactionDate = DateTime.UtcNow
                };

                _context.LedgerTransactions.Add(transaction2);
                await _context.SaveChangesAsync();

                otherLedger.Balance = otherLedger.Balance + (int)dto.Amount;
                _context.LedgerAccounts.Update(otherLedger);
                await _context.SaveChangesAsync();



                var transaction3 = new LedgerTransaction
                {
                    LedgerAccountId = 1,
                    MemberId = dto.MemberId,
                    LoanId = dto.LoanId,
                    Debit = 0,
                    Credit = dto.Amount,
                    ParticularId = 1,
                    PayId = nextPayId,
                    SocietyId = dto.SocietyId,
                    BankId = (int)dto.BankId,
                    Description = dto.Narration,
                    VoucherId = voucher.VoucherId,
                    TransactionDate = DateTime.UtcNow
                };

                _context.LedgerTransactions.Add(transaction3);
                await _context.SaveChangesAsync();

                 societyLedger.Balance += dto.Amount; // assuming Credit increases balance
                _context.LedgerAccounts.Update( societyLedger);
                await _context.SaveChangesAsync();
            }
            else if(dto.VoucherType == "Journel"){
                var transaction4 = new LedgerTransaction
                {
                    LedgerAccountId = dto.ParticularId,
                    MemberId = dto.MemberId,
                    LoanId = dto.LoanId,
                    Debit = 0,
                    Credit = dto.Amount,
                    ParticularId = dto.ParticularId,
                    PayId = nextPayId,
                    SocietyId = dto.SocietyId,
                    BankId = (int)dto.BankId,
                    Description = dto.Narration,
                    VoucherId = voucher.VoucherId,
                    TransactionDate = DateTime.UtcNow
                };

                _context.LedgerTransactions.Add(transaction4);
                await _context.SaveChangesAsync();

                otherLedger.Balance = otherLedger.Balance + dto.Amount;
                _context.LedgerAccounts.Update(otherLedger);
                await _context.SaveChangesAsync();



                var transaction5 = new LedgerTransaction
                {
                    LedgerAccountId = 1,
                    MemberId = dto.MemberId,
                    LoanId = 0,
                    Debit = dto.Amount,
                    Credit = dto.Amount,
                    ParticularId = 1,
                    PayId = nextPayId,
                    SocietyId = dto.SocietyId,
                    BankId = (int)dto.BankId,
                    Description = dto.Narration,
                    VoucherId = voucher.VoucherId,
                    TransactionDate = DateTime.UtcNow
                };

                _context.LedgerTransactions.Add(transaction5);
                await _context.SaveChangesAsync();

                 societyLedger.Balance -= dto.Amount; // assuming Credit increases balance
                _context.LedgerAccounts.Update( societyLedger);
                await _context.SaveChangesAsync();
            }


            
            

            return voucher;
        }
    }
}

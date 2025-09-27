using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintcsApi.Services.Implementations
{
    public class LoanService : ILoanService
    {
        private readonly AppDbContext _context;
        private readonly ILedgerService _ledgerService;
        private readonly IVoucherService _voucherService;

        public LoanService(AppDbContext context, ILedgerService ledgerService, IVoucherService voucherService)
        {
            _context = context;
            _ledgerService = ledgerService;
            _voucherService = voucherService;
        }

        public async Task<(bool Success, string Message, LoanDto? Data)> CreateLoanAsync(LoanCreateUpdateDto dto)
        {
            var loanType = await _context.LoanTypes.FindAsync(dto.LoanTypeId);
            if (loanType == null) return (false, "Loan type not found", null);

            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null) return (false, "Member not found", null);

            var existingLoan = await _context.Loans
                .FirstOrDefaultAsync(l => l.MemberId == dto.MemberId &&
                                          l.LoanTypeId == dto.LoanTypeId &&
                                          l.Status == "Active");

            if (existingLoan != null)
                return (false, $"Member already has an active {loanType.Name} loan", null);

            // PaymentMode validations
            if (dto.PaymentMode == "Cheque")
            {
                if (string.IsNullOrWhiteSpace(dto.Bank) || string.IsNullOrWhiteSpace(dto.ChequeNo) || dto.ChequeDate == null)
                    return (false, "Bank, ChequeNo and ChequeDate are required for Cheque payments", null);
            }
            else
            {
                dto.Bank = null;
                dto.ChequeNo = null;
                dto.ChequeDate = null;
            }

            if (!loanType.Name.Equals("General Loan", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(dto.Purpose) || string.IsNullOrWhiteSpace(dto.AuthorizedBy))
                    return (false, "Purpose and AuthorizedBy are required for this loan type", null);
            }
            else
            {
                dto.Purpose = null;
                dto.AuthorizedBy = null;
            }

            var loan = new Loan
            {
                SocietyId = dto.SocietyId,
                MemberId = dto.MemberId,
                LoanTypeId = dto.LoanTypeId,
                LoanDate = dto.LoanDate,
                LoanAmount = dto.LoanAmount,
                PreviousLoan = dto.PreviousLoan,
                Installments = dto.Installments,
                Purpose = dto.Purpose,
                AuthorizedBy = dto.AuthorizedBy,
                PaymentMode = dto.PaymentMode,
                Bank = dto.Bank,
                ChequeNo = dto.ChequeNo,
                ChequeDate = dto.ChequeDate,
                Status = dto.Status,
                NetLoan = dto.NetLoan,
                InstallmentAmount = dto.InstallmentAmount,
                NewLoanShare = dto.NewLoanShare,
                PayAmount = dto.PayAmount
            };

            // Update member share
            if (loan.NewLoanShare > 0)
            {
                member.Share += loan.NewLoanShare;
                _context.Members.Update(member);

                var shareLedger = await _context.LedgerAccounts
                    .FirstOrDefaultAsync(l => l.MemberId == member.Id && l.AccountName == "Share Ledger");

                if (shareLedger != null)
                {
                    await _ledgerService.RecordTransactionAsync(new LedgerTransactionDto
                    {
                        LedgerAccountId = shareLedger.LedgerAccountId,
                        MemberId = member.Id,
                        Credit = loan.NewLoanShare,
                        Debit = 0,
                        Description = "New Loan Share added"
                    });
                }
            }

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            // Loan ledger entries
            var loanLedger = await _context.LedgerAccounts
                .FirstOrDefaultAsync(l => l.MemberId == member.Id && l.AccountName == $"{loanType.Name} Loan Ledger");

            if (loanLedger != null)
            {
                await _ledgerService.RecordTransactionAsync(new LedgerTransactionDto
                {
                    LedgerAccountId = loanLedger.LedgerAccountId,
                    MemberId = member.Id,
                    LoanId = loan.LoanId,
                    Credit = loan.NetLoan,
                    Debit = 0,
                    Description = "Loan disbursed"
                });

                var cashLedger = await _context.LedgerAccounts
                    .FirstOrDefaultAsync(l => l.AccountName == "Cash Ledger" || l.AccountName == "Bank Ledger");

                if (cashLedger != null)
                {
                    await _ledgerService.RecordTransactionAsync(new LedgerTransactionDto
                    {
                        LedgerAccountId = cashLedger.LedgerAccountId,
                        MemberId = member.Id,
                        LoanId = loan.LoanId,
                        Debit = loan.NetLoan,
                        Credit = 0,
                        Description = "Cash/Bank paid for loan"
                    });

                    var loanVoucher = new CreateVoucherDto
                    {
                        VoucherType = "LoanDisbursement",
                        MemberId = member.Id,
                        LoanId = loan.LoanId,
                        Narration = $"Loan disbursed to {member.Name}"
                    };
                    loanVoucher.Entries.Add(new VoucherEntryDto
                    {
                        LedgerAccountId = loanLedger.LedgerAccountId,
                        Credit = loan.NetLoan,
                        Debit = 0,
                        Description = "Loan disbursed"
                    });
                    loanVoucher.Entries.Add(new VoucherEntryDto
                    {
                        LedgerAccountId = cashLedger.LedgerAccountId,
                        Debit = loan.NetLoan,
                        Credit = 0,
                        Description = "Cash/Bank paid for loan"
                    });

                    await _voucherService.CreateVoucherAsync(loanVoucher);
                }
            }

            return (true, "Loan created successfully", MapToDto(loan, member.Name, loanType.Name));
        }

        public async Task<(bool Success, string Message, IEnumerable<LoanDto>? Data)> GetLoansByMemberAsync(int memberId)
        {
            var loans = await _context.Loans
                .Where(l => l.MemberId == memberId)
                .Include(l => l.LoanType)
                .Include(l => l.Member)
                .ToListAsync();

            if (!loans.Any()) return (false, "No loans found for this member", null);

            var loanDtos = loans.Select(l => MapToDto(l, l.Member?.Name ?? "", l.LoanType?.Name ?? "")).ToList();
            return (true, "Loans retrieved successfully", loanDtos);
        }

        public async Task<(bool Success, string Message, IEnumerable<LoanDto>? Data)> GetLoansBySocietyAsync(int societyId)
        {
            var loans = await _context.Loans
                .Include(l => l.Member)
                .Include(l => l.LoanType)
                .Where(l => l.SocietyId == societyId)
                .ToListAsync();

            if (!loans.Any()) return (false, "No loans found", null);

            var result = loans.Select(l => MapToDto(l, l.Member?.Name ?? "", l.LoanType?.Name ?? "")).ToList();
            return (true, "Loans fetched successfully", result);
        }

        public async Task<(bool Success, string Message, LoanDto? Data)> GetLoanByIdAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Member)
                .Include(l => l.LoanType)
                .FirstOrDefaultAsync(l => l.LoanId == loanId);

            if (loan == null) return (false, "Loan not found", null);

            return (true, "Loan fetched successfully", MapToDto(loan, loan.Member?.Name ?? "", loan.LoanType?.Name ?? ""));
        }

        public async Task<(bool Success, string Message)> UpdateLoanAsync(int loanId, LoanCreateUpdateDto dto)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null) return (false, "Loan not found");

            loan.LoanDate = dto.LoanDate;
            loan.LoanAmount = dto.LoanAmount;
            loan.PreviousLoan = dto.PreviousLoan;
            loan.Installments = dto.Installments;
            loan.Purpose = dto.Purpose;
            loan.AuthorizedBy = dto.AuthorizedBy;
            loan.PaymentMode = dto.PaymentMode;
            loan.Bank = dto.Bank;
            loan.ChequeNo = dto.ChequeNo;
            loan.ChequeDate = dto.ChequeDate;
            loan.Status = dto.Status;
            loan.NetLoan = dto.NetLoan;
            loan.InstallmentAmount = dto.InstallmentAmount;
            loan.NewLoanShare = dto.NewLoanShare;
            loan.PayAmount = dto.PayAmount;
            loan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return (true, "Loan updated successfully");
        }

        public async Task<(bool Success, string Message)> DeleteLoanAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null) return (false, "Loan not found");

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return (true, "Loan deleted successfully");
        }

        private LoanDto MapToDto(Loan loan, string memberName, string loanTypeName)
        {
            return new LoanDto
            {
                LoanId = loan.LoanId,
                SocietyId = loan.SocietyId,
                MemberId = loan.MemberId,
                LoanTypeId = loan.LoanTypeId,
                LoanTypeName = loanTypeName,
                MemberName = memberName,
                LoanDate = loan.LoanDate,
                LoanAmount = loan.LoanAmount,
                Installments = loan.Installments,
                Purpose = loan.Purpose,
                AuthorizedBy = loan.AuthorizedBy,
                PaymentMode = loan.PaymentMode,
                Status = loan.Status,
                NetLoan = loan.NetLoan,
                InstallmentAmount = loan.InstallmentAmount,
                NewLoanShare = loan.NewLoanShare,
                PayAmount = loan.PayAmount,
                PreviousLoan = loan.PreviousLoan
            };
        }
    }
}

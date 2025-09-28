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
        private readonly ILoanTypeService _loanTypeService;
        private readonly ILedgerService _ledgerService;
        private readonly IVoucherService _voucherService;

        public LoanService(AppDbContext context, ILoanTypeService loanTypeService, ILedgerService ledgerService, IVoucherService voucherService)
        {
            _context = context;
            _loanTypeService = loanTypeService;
            _ledgerService = ledgerService;
            _voucherService = voucherService;
        }

        // public async Task<ApiResponse<LoanDto>> CreateLoanAsync(LoanCreateUpdateDto dto)
        // {
        //     try
        //     {
        //         Console.WriteLine("Starting loan creation...");

        //         var loan = new Loan
        //         {
        //             SocietyId = dto.SocietyId,
        //             MemberId = dto.MemberId,
        //             LoanTypeId = dto.LoanTypeId,
        //             LoanDate = dto.LoanDate,
        //             LoanAmount = dto.LoanAmount,
        //             Installments = dto.Installments,
        //             Purpose = dto.Purpose,
        //             AuthorizedBy = dto.AuthorizedBy,
        //             PaymentMode = dto.PaymentMode,
        //             Status = "Active",
        //             Bank = dto.Bank,
        //             ChequeNo = dto.ChequeNo,
        //             ChequeDate = dto.ChequeDate,
        //             NetLoan = dto.NetLoan,
        //             InstallmentAmount = dto.InstallmentAmount,
        //             NewLoanShare = dto.NewLoanShare,
        //             PayAmount = dto.PayAmount,
        //             PreviousLoan = dto.PreviousLoan,
        //             CreatedAt = DateTime.UtcNow,
        //             UpdatedAt = DateTime.UtcNow
        //         };

        //         Console.WriteLine($"Loan object created: MemberId={loan.MemberId}, Amount={loan.LoanAmount}");

        //         _context.Loans.Add(loan);
        //         await _context.SaveChangesAsync();
        //         Console.WriteLine($"Loan saved with LoanId={loan.LoanId}");

        //         // Get the saved loan with includes
        //         loan = await _context.Loans
        //             .Include(l => l.Member)
        //             .Include(l => l.LoanType)
        //             .FirstOrDefaultAsync(l => l.LoanId == loan.LoanId);

        //         Console.WriteLine("Loan retrieved with related entities:");
        //         Console.WriteLine($"Member: {loan.Member?.Name}, LoanTypeId: {loan.LoanTypeId}");

        //         // Get LoanType from service
        //         var loanTypeResponse = await _loanTypeService.GetLoanTypeByIdAsync(loan.LoanTypeId);
        //         var loanType = loanTypeResponse.Data;
        //         if (loanType == null)
        //         {
        //             Console.WriteLine($"LoanType not found for LoanTypeId={loan.LoanTypeId}");
        //         }
        //         else
        //         {
        //             Console.WriteLine($"LoanType fetched: {loanType.Name}");

        //             // Split loan type name into words
        //             var loanTypeWords = loanType.Name
        //                 .ToLower()
        //                 .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //             Console.WriteLine($"LoanType words: {string.Join(", ", loanTypeWords)}");

        //             // Get all ledger accounts for the member
        //             var ledgerAccounts = await _context.LedgerAccounts
        //                 .Where(la => la.MemberId == loan.MemberId)
        //                 .ToListAsync();

        //             // Fuzzy match ledger account
        //             LedgerAccount matchedLedgerAccount = null;
        //             foreach (var la in ledgerAccounts)
        //             {
        //                 var accountName = la.AccountName?.ToLower() ?? "";
        //                 if (loanTypeWords.All(w => accountName.Contains(w)))
        //                 {
        //                     matchedLedgerAccount = la;
        //                     break;
        //                 }
        //             }

        //             if (matchedLedgerAccount != null)
        //             {
        //                 Console.WriteLine($"Matched LedgerAccount: LedgerAccountId={matchedLedgerAccount.LedgerAccountId}, AccountName={matchedLedgerAccount.AccountName}");
        //             }
        //             else
        //             {
        //                 Console.WriteLine($"No matching LedgerAccount found for LoanType '{loanType.Name}'");
        //             }
        //         }

        //         // var voucher = new Voucher
        //         // {
        //         //     VoucherType = dto.VoucherType,  here we can set Payment by default because it is loan given by society to member
        //         //     MemberId = dto.MemberId,  we can get dto.MemberId from loan.MemberId                //     LoanId = dto.LoanId,
        //         //     Narration = dto.Narration,  we can use purpose as narration
        //         //     Amount = dto.Amount, we can use loan.LoanAmount as amount
        //         //     BankId = dto.BankId, we can use loan.SocietyBank as bankId
        //         //     ChecqueNumber = dto.ChequeNumber, we can use loan.ChequeNo as ChequeNumber
        //         //     ChecqueDate = dto.ChequeDate, we can use loan.ChequeDate as ChequeDate
        //         //     VoucherDate = dto.VoucherDate, we can use loan.LoanDate as VoucherDate
        //         //     LedgerTransactionId = dto.LedgerTransactionId, we can use 0 initially
        //         //     PayId = dto.PayId, we have to generate new payId for voucher
        //         //     ParticularId = dto.ParticularId, we have to get from LedgerAccounts
        //         //     SocietyId = dto.SocietyId,
        //         //     CreatedAt = DateTime.UtcNow
        //         // };
        //         _context.Vouchers.Add(voucher);
        //         await _context.SaveChangesAsync();
        //         Console.WriteLine("Voucher creation skipped (commented).");

        //         return ApiResponse<LoanDto>.SuccessResponse(MapToDto(loan), "Loan created successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error creating loan: {ex.Message}");
        //         return ApiResponse<LoanDto>.ErrorResponse($"Error creating loan: {ex.Message}");
        //     }
        // }

        public async Task<ApiResponse<LoanDto>> CreateLoanAsync(LoanCreateUpdateDto dto)
        {
            try
            {
                Console.WriteLine("=== Starting Loan Creation ===");

                // 1️⃣ Create Loan object
                var loan = new Loan
                {
                    SocietyId = dto.SocietyId,
                    MemberId = dto.MemberId,
                    LoanTypeId = dto.LoanTypeId,
                    LoanDate = dto.LoanDate,
                    LoanAmount = dto.LoanAmount,
                    Installments = dto.Installments,
                    Purpose = dto.Purpose,
                    AuthorizedBy = dto.AuthorizedBy,
                    PaymentMode = dto.PaymentMode,
                    Status = "Active",
                    Bank = dto.Bank,
                    ChequeNo = dto.ChequeNo,
                    ChequeDate = dto.ChequeDate,
                    NetLoan = dto.NetLoan,
                    InstallmentAmount = dto.InstallmentAmount,
                    NewLoanShare = dto.NewLoanShare,
                    PayAmount = dto.PayAmount,
                    PreviousLoan = dto.PreviousLoan,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Console.WriteLine($"Loan object created: MemberId={loan.MemberId}, Amount={loan.LoanAmount}");

                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
                // Console.WriteLine($"Loan saved with LoanId={loan.LoanId}");

                // 2️⃣ Reload loan with related entities
                loan = await _context.Loans
                    .Include(l => l.Member)
                    .Include(l => l.LoanType)
                    .FirstOrDefaultAsync(l => l.LoanId == loan.LoanId);

                // Console.WriteLine($"Loan retrieved: Member={loan.Member?.Name}, LoanTypeId={loan.LoanTypeId}");

                // 3️⃣ Get LoanType from service
                var loanTypeResponse = await _loanTypeService.GetLoanTypeByIdAsync(loan.LoanTypeId);
                var loanType = loanTypeResponse.Data;

                LedgerAccount matchedLedgerAccount = null;

                if (loanType == null)
                {
                    Console.WriteLine($"LoanType not found for LoanTypeId={loan.LoanTypeId}");
                }
                else
                {
                    // Console.WriteLine($"LoanType fetched: {loanType.Name}");

                    // Split LoanType name into words for fuzzy matching
                    var loanTypeWords = loanType.Name
                        .ToLower()
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    // Console.WriteLine($"LoanType words: {string.Join(", ", loanTypeWords)}");

                    // 4️⃣ Get all LedgerAccounts for the member
                    var ledgerAccounts = await _context.LedgerAccounts
                        .Where(la => la.MemberId == loan.MemberId)
                        .ToListAsync();

                    // Console.WriteLine($"Total ledger accounts fetched for MemberId={loan.MemberId}: {ledgerAccounts.Count}");

                    // 5️⃣ Fuzzy match ledger account
                    foreach (var la in ledgerAccounts)
                    {
                        var accountName = la.AccountName?.ToLower() ?? "";
                        // Console.WriteLine($"Checking LedgerAccountId={la.LedgerAccountId}, AccountName={la.AccountName}");

                        if (loanTypeWords.All(w => accountName.Contains(w)))
                        {
                            matchedLedgerAccount = la;
                            // Console.WriteLine($"Matched LedgerAccount: LedgerAccountId={la.LedgerAccountId}, AccountName={la.AccountName}");
                            break;
                        }
                    }

                    if (matchedLedgerAccount == null)
                    {
                        Console.WriteLine($"No matching LedgerAccount found for LoanType '{loanType.Name}'");
                    }
                }

                int nextPayId = 1;
                var lastVoucher = await _context.Vouchers
                    .OrderByDescending(v => v.PayId)
                    .FirstOrDefaultAsync();
                if (lastVoucher != null)
                    nextPayId = lastVoucher.PayId + 1;


                // 6️⃣ Create Voucher
                var voucher = new Voucher
                {
                    VoucherType = "Payment",  // default: Payment
                    MemberId = loan.MemberId,
                    LoanId = loan.LoanId,
                    Narration = loan.Purpose ?? "Loan Disbursement",
                    Amount = loan.LoanAmount,
                    BankId = dto.Bank,
                    ChecqueNumber = dto.ChequeNo ?? null,
                    ChecqueDate = loan.ChequeDate ?? null,
                    VoucherDate = loan.LoanDate, 
                    PayId = nextPayId, // auto create it
                    ParticularId = matchedLedgerAccount?.LedgerAccountId ?? 0,
                    SocietyId = dto.SocietyId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();
                // Console.WriteLine($"Voucher created successfully: VoucherId={voucher.VoucherId}");

                var transaction = new LedgerTransaction
                {
                    LedgerAccountId = matchedLedgerAccount?.LedgerAccountId ?? 0,
                    MemberId = loan.MemberId,
                    LoanId = loan.LoanId,
                    Debit = 0,
                    Credit = loan.LoanAmount,
                    ParticularId = matchedLedgerAccount?.LedgerAccountId ?? 0,
                    PayId = nextPayId,
                    SocietyId = dto.SocietyId,
                    BankId = dto.Bank,
                    Description = loan.Purpose ?? "Loan Disbursement",
                    VoucherId = voucher.VoucherId,
                    TransactionDate = DateTime.UtcNow
                };

                _context.LedgerTransactions.Add(transaction);
                await _context.SaveChangesAsync();

                matchedLedgerAccount.Balance += loan.LoanAmount; // assuming Credit increases balance
                _context.LedgerAccounts.Update(matchedLedgerAccount);
                await _context.SaveChangesAsync();

                // now i need to add amount to ledger account using the transaction.LedgerAccountId

                var societyLedger = await _context.LedgerAccounts
                    .FirstOrDefaultAsync(la => la.AccountName == "Society Account");

                var transaction1 = new LedgerTransaction
                {
                    LedgerAccountId = societyLedger.LedgerAccountId ?? 1,
                    MemberId = loan.MemberId,
                    LoanId = loan.LoanId,
                    Debit = loan.LoanAmount,
                    Credit = 0,
                    ParticularId = societyLedger.LedgerAccountId ?? 1,
                    PayId = nextPayId,
                    SocietyId = dto.SocietyId,
                    BankId = dto.Bank,
                    Description = loan.Purpose ?? "Loan Disbursement",
                    VoucherId = voucher.VoucherId,
                    TransactionDate = DateTime.UtcNow
                };

                _context.LedgerTransactions.Add(transaction1);
                await _context.SaveChangesAsync();

                
                
                societyLedger.Balance -= loan.LoanAmount; // assuming Credit increases balance
                _context.LedgerAccounts.Update(societyLedger);
                await _context.SaveChangesAsync();

                // now i need to add amount to ledger account using the transaction1.LedgerAccountId

                // 7️⃣ Return success response
                return ApiResponse<LoanDto>.SuccessResponse(MapToDto(loan), "Loan and Voucher created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating loan: {ex.Message}");
                return ApiResponse<LoanDto>.ErrorResponse($"Error creating loan: {ex.Message}");
            }
        }



        public async Task<ApiResponse<IEnumerable<LoanDto>>> GetLoansBySocietyAsync(int societyId)
        {
            var loans = await _context.Loans
                .Where(l => l.SocietyId == societyId)
                .Include(l => l.Member)
                .Include(l => l.LoanType)
                .ToListAsync();

            var result = loans.Select(MapToDto).ToList();
            return ApiResponse<IEnumerable<LoanDto>>.SuccessResponse(result, "Loans fetched successfully");
        }

        public async Task<ApiResponse<LoanDto>> GetLoanByIdAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Member)
                .Include(l => l.LoanType)
                .FirstOrDefaultAsync(l => l.LoanId == loanId);

            if (loan == null)
                return ApiResponse<LoanDto>.ErrorResponse("Loan not found");

            return ApiResponse<LoanDto>.SuccessResponse(MapToDto(loan), "Loan fetched successfully");
        }

        public async Task<ApiResponse<object>> UpdateLoanAsync(int loanId, LoanCreateUpdateDto dto)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
                return ApiResponse<object>.ErrorResponse("Loan not found");

            loan.SocietyId = dto.SocietyId;
            loan.MemberId = dto.MemberId;
            loan.LoanTypeId = dto.LoanTypeId;
            loan.LoanDate = dto.LoanDate;
            loan.LoanAmount = dto.LoanAmount;
            loan.Installments = dto.Installments;
            loan.Purpose = dto.Purpose;
            loan.AuthorizedBy = dto.AuthorizedBy;
            loan.PaymentMode = dto.PaymentMode;
            loan.Status = dto.Status;
            loan.Bank = dto.Bank;
            loan.ChequeNo = dto.ChequeNo;
            loan.ChequeDate = dto.ChequeDate;
            loan.NetLoan = dto.NetLoan;
            loan.InstallmentAmount = dto.InstallmentAmount;
            loan.NewLoanShare = dto.NewLoanShare;
            loan.PayAmount = dto.PayAmount;
            loan.PreviousLoan = dto.PreviousLoan;
            loan.UpdatedAt = DateTime.UtcNow;

            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();

            return ApiResponse<object>.SuccessResponse(null, "Loan updated successfully");
        }

        public async Task<ApiResponse<object>> DeleteLoanAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
                return ApiResponse<object>.ErrorResponse("Loan not found");

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return ApiResponse<object>.SuccessResponse(null, "Loan deleted successfully");
        }

        public async Task<ApiResponse<IEnumerable<LoanDto>>> GetLoansByMemberAsync(int memberId)
        {
            var loans = await _context.Loans
                .Where(l => l.MemberId == memberId)
                .Include(l => l.Member)
                .Include(l => l.LoanType)
                .ToListAsync();

            var result = loans.Select(MapToDto).ToList();
            return ApiResponse<IEnumerable<LoanDto>>.SuccessResponse(result, "Loans fetched successfully");
        }

        private LoanDto MapToDto(Loan loan)
        {
            return new LoanDto
            {
                LoanId = loan.LoanId,
                SocietyId = loan.SocietyId,
                MemberId = loan.MemberId,
                LoanTypeId = loan.LoanTypeId,
                LoanTypeName = loan.LoanType?.Name ?? string.Empty,
                MemberName = loan.Member?.Name ?? string.Empty,
                LoanDate = loan.LoanDate,
                LoanAmount = loan.LoanAmount,
                Installments = loan.Installments,
                Purpose = loan.Purpose,
                AuthorizedBy = loan.AuthorizedBy,
                PaymentMode = loan.PaymentMode,
                Status = loan.Status,
                Bank = loan.Bank,
                ChequeNo = loan.ChequeNo,
                ChequeDate = loan.ChequeDate,
                NetLoan = loan.NetLoan,
                InstallmentAmount = loan.InstallmentAmount,
                NewLoanShare = loan.NewLoanShare,
                PayAmount = loan.PayAmount,
                PreviousLoan = loan.PreviousLoan
            };
        }
    }
}

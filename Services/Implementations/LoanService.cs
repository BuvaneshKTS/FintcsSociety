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

        public LoanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<LoanDto>> CreateLoanAsync(LoanCreateUpdateDto dto)
        {
            try
            {
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

                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();

                return ApiResponse<LoanDto>.SuccessResponse(MapToDto(loan), "Loan created successfully");
            }
            catch (Exception ex)
            {
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

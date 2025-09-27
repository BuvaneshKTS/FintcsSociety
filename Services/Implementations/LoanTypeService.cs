using Microsoft.EntityFrameworkCore;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintcsApi.Services.Implementations
{
    public class LoanTypeService : ILoanTypeService
    {
        private readonly AppDbContext _context;

        public LoanTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<LoanTypeDto>> CreateLoanTypeAsync(LoanTypeDto dto)
        {
            var society = await _context.Societies
                .Include(s => s.Members)
                .FirstOrDefaultAsync(s => s.Id == dto.SocietyId);

            if (society == null)
                return ApiResponse<LoanTypeDto>.ErrorResponse("Society not found.");

            var exists = await _context.LoanTypes.AnyAsync(lt =>
                lt.SocietyId == dto.SocietyId && lt.Name.ToLower() == dto.Name.ToLower());

            if (exists)
                return ApiResponse<LoanTypeDto>.ErrorResponse("LoanType with same name already exists for this society.");

            var loanType = new LoanType
            {
                SocietyId = dto.SocietyId,
                Name = dto.Name,
                InterestPercent = dto.InterestPercent,
                LimitAmount = dto.LimitAmount,
                CompulsoryDeposit = dto.CompulsoryDeposit,
                OptionalDeposit = dto.OptionalDeposit,
                ShareAmount = dto.ShareAmount,
                XTimes = dto.XTimes,
                CreatedAt = DateTime.UtcNow
            };

            _context.LoanTypes.Add(loanType);

            // Create ledger accounts for each member
            if (society.Members != null && society.Members.Any())
            {
                foreach (var member in society.Members)
                {
                    var ledgerExists = await _context.LedgerAccounts
                        .AnyAsync(l => l.MemberId == member.Id && l.AccountName == $"{loanType.Name} Loan Ledger");

                    if (!ledgerExists)
                    {
                        _context.LedgerAccounts.Add(new LedgerAccount
                        {
                            MemberId = member.Id,
                            AccountName = $"{loanType.Name} Loan Ledger",
                            Balance = 0,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return ApiResponse<LoanTypeDto>.SuccessResponse(MapToDto(loanType));
        }

        public async Task<ApiResponse<LoanTypeDto>> GetLoanTypeByIdAsync(int id)
        {
            var loanType = await _context.LoanTypes.FindAsync(id);
            if (loanType == null)
                return ApiResponse<LoanTypeDto>.ErrorResponse("LoanType not found.");

            return ApiResponse<LoanTypeDto>.SuccessResponse(MapToDto(loanType));
        }

        public async Task<ApiResponse<List<LoanTypeDto>>> GetAllLoanTypesBySocietyAsync(int societyId)
        {
            var list = await _context.LoanTypes
                .Where(lt => lt.SocietyId == societyId)
                .ToListAsync();

            return ApiResponse<List<LoanTypeDto>>.SuccessResponse(list.Select(MapToDto).ToList());
        }

        public async Task<ApiResponse<LoanTypeDto>> UpdateLoanTypeAsync(int id, LoanTypeDto dto)
        {
            var loanType = await _context.LoanTypes.FindAsync(id);
            if (loanType == null)
                return ApiResponse<LoanTypeDto>.ErrorResponse("LoanType not found.");

            loanType.Name = dto.Name;
            loanType.InterestPercent = dto.InterestPercent;
            loanType.LimitAmount = dto.LimitAmount;
            loanType.CompulsoryDeposit = dto.CompulsoryDeposit;
            loanType.OptionalDeposit = dto.OptionalDeposit;
            loanType.ShareAmount = dto.ShareAmount;
            loanType.XTimes = dto.XTimes;
            loanType.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResponse<LoanTypeDto>.SuccessResponse(MapToDto(loanType));
        }

        public async Task<ApiResponse<bool>> DeleteLoanTypeAsync(int id)
        {
            var loanType = await _context.LoanTypes.FindAsync(id);
            if (loanType == null)
                return ApiResponse<bool>.ErrorResponse("LoanType not found.");

            _context.LoanTypes.Remove(loanType);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "LoanType deleted successfully");
        }

        private LoanTypeDto MapToDto(LoanType lt)
        {
            return new LoanTypeDto
            {
                LoanTypeId = lt.LoanTypeId,
                SocietyId = lt.SocietyId,
                Name = lt.Name,
                InterestPercent = lt.InterestPercent,
                LimitAmount = lt.LimitAmount,
                CompulsoryDeposit = lt.CompulsoryDeposit,
                OptionalDeposit = lt.OptionalDeposit,
                ShareAmount = lt.ShareAmount,
                XTimes = lt.XTimes
            };
        }
    }
}

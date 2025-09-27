using Microsoft.EntityFrameworkCore;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Services.Implementations;

public class BankAccountService : IBankAccountService
{
    private readonly AppDbContext _context;

    public BankAccountService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<BankAccountDto>> CreateBankAccountAsync(BankAccountCreateUpdateDto dto)
    {
        var society = await _context.Societies.FindAsync(dto.SocietyId);
        if (society == null)
            return ApiResponse<BankAccountDto>.ErrorResponse("Society not found.");

        var exists = await _context.SocietyBankAccounts.AnyAsync(ba =>
            ba.SocietyId == dto.SocietyId &&
            ba.BankName.ToLower() == dto.BankName.ToLower() &&
            ba.AccountNumber == dto.AccountNumber);

        if (exists)
            return ApiResponse<BankAccountDto>.ErrorResponse("Bank account already exists for this society.");

        var account = new SocietyBankAccount
        {
            SocietyId = dto.SocietyId,
            BankName = dto.BankName,
            AccountNumber = dto.AccountNumber,
            IFSC = dto.IFSC,
            Branch = dto.Branch,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _context.SocietyBankAccounts.Add(account);
        await _context.SaveChangesAsync();

        return ApiResponse<BankAccountDto>.SuccessResponse(MapToDto(account));
    }

    public async Task<ApiResponse<BankAccountDto>> GetBankAccountByIdAsync(int id)
    {
        var account = await _context.SocietyBankAccounts.FindAsync(id);
        if (account == null)
            return ApiResponse<BankAccountDto>.ErrorResponse("Bank account not found.");

        return ApiResponse<BankAccountDto>.SuccessResponse(MapToDto(account));
    }

    public async Task<ApiResponse<List<BankAccountDto>>> GetAllBankAccountsBySocietyAsync(int societyId)
    {
        var list = await _context.SocietyBankAccounts
            .Where(ba => ba.SocietyId == societyId)
            .ToListAsync();

        return ApiResponse<List<BankAccountDto>>.SuccessResponse(list.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<BankAccountDto>> UpdateBankAccountAsync(int id, BankAccountCreateUpdateDto dto)
    {
        var account = await _context.SocietyBankAccounts.FindAsync(id);
        if (account == null)
            return ApiResponse<BankAccountDto>.ErrorResponse("Bank account not found.");

        account.BankName = dto.BankName;
        account.AccountNumber = dto.AccountNumber;
        account.IFSC = dto.IFSC;
        account.Branch = dto.Branch;
        account.Notes = dto.Notes;
        account.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ApiResponse<BankAccountDto>.SuccessResponse(MapToDto(account));
    }

    public async Task<ApiResponse<bool>> DeleteBankAccountAsync(int id)
    {
        var account = await _context.SocietyBankAccounts.FindAsync(id);
        if (account == null)
            return ApiResponse<bool>.ErrorResponse("Bank account not found.");

        _context.SocietyBankAccounts.Remove(account);
        await _context.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Bank account deleted successfully");
    }

    private BankAccountDto MapToDto(SocietyBankAccount ba)
    {
        return new BankAccountDto
        {
            Id = ba.Id,
            SocietyId = ba.SocietyId,
            BankName = ba.BankName,
            AccountNumber = ba.AccountNumber,
            IFSC = ba.IFSC,
            Branch = ba.Branch,
            Notes = ba.Notes
        };
    }
}

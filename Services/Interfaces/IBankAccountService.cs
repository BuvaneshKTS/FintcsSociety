using FintcsApi.DTOs;
using FintcsApi.Models;


namespace FintcsApi.Services.Interfaces;

public interface IBankAccountService
{
    Task<ApiResponse<BankAccountDto>> CreateBankAccountAsync(BankAccountCreateUpdateDto dto);
    Task<ApiResponse<BankAccountDto>> GetBankAccountByIdAsync(int id);
    Task<ApiResponse<List<BankAccountDto>>> GetAllBankAccountsBySocietyAsync(int societyId);
    Task<ApiResponse<BankAccountDto>> UpdateBankAccountAsync(int id, BankAccountCreateUpdateDto dto);
    Task<ApiResponse<bool>> DeleteBankAccountAsync(int id);
}

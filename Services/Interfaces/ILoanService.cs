using FintcsApi.DTOs;
using FintcsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FintcsApi.Services.Interfaces
{
    public interface ILoanService
    {
        Task<(bool Success, string Message, LoanDto? Data)> CreateLoanAsync(LoanCreateUpdateDto dto);
        Task<(bool Success, string Message, IEnumerable<LoanDto>? Data)> GetLoansBySocietyAsync(int societyId);
        Task<(bool Success, string Message, LoanDto? Data)> GetLoanByIdAsync(int loanId);
        Task<(bool Success, string Message)> UpdateLoanAsync(int loanId, LoanCreateUpdateDto dto);
        Task<(bool Success, string Message)> DeleteLoanAsync(int loanId);
        Task<(bool Success, string Message, IEnumerable<LoanDto>? Data)> GetLoansByMemberAsync(int memberId);
    }
}

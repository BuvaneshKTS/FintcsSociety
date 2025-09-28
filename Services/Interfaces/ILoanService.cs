using FintcsApi.DTOs;
using FintcsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FintcsApi.Services.Interfaces
{
    public interface ILoanService
    {
        Task<ApiResponse<LoanDto>> CreateLoanAsync(LoanCreateUpdateDto dto);
        Task<ApiResponse<IEnumerable<LoanDto>>> GetLoansBySocietyAsync(int societyId);
        Task<ApiResponse<LoanDto>> GetLoanByIdAsync(int loanId);
        Task<ApiResponse<object>> UpdateLoanAsync(int loanId, LoanCreateUpdateDto dto);
        Task<ApiResponse<object>> DeleteLoanAsync(int loanId);
        Task<ApiResponse<IEnumerable<LoanDto>>> GetLoansByMemberAsync(int memberId);
    }
}

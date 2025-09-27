using FintcsApi.DTOs;
using FintcsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FintcsApi.Services.Interfaces
{
    public interface ILoanTypeService
    {
        Task<ApiResponse<LoanTypeDto>> CreateLoanTypeAsync(LoanTypeDto dto);
        Task<ApiResponse<LoanTypeDto>> GetLoanTypeByIdAsync(int id);
        Task<ApiResponse<List<LoanTypeDto>>> GetAllLoanTypesBySocietyAsync(int societyId);
        Task<ApiResponse<LoanTypeDto>> UpdateLoanTypeAsync(int id, LoanTypeDto dto);
        Task<ApiResponse<bool>> DeleteLoanTypeAsync(int id);
    }
}

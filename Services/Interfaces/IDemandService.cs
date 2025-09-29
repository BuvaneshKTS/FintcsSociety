using System.Collections.Generic;
using System.Threading.Tasks;
using FintcsApi.DTOs;
using FintcsApi.Models;

namespace FintcsApi.Services.Interfaces
{
    public interface IDemandService
    {
        // Create a demand
        Task<ApiResponse<bool>> CreateDemandAsync(DemandCreateDto dto);

        // Get demands by month and year
        Task<ApiResponse<List<DemandViewDto>>> GetDemandByMonthYearAsync(int year, int month);

        // Delete demands by month and year
        Task<ApiResponse<bool>> DeleteDemandAsync(int year, int month);
    }
}

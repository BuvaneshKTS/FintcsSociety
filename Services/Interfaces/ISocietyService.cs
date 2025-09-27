using FintcsApi.DTOs;
using FintcsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FintcsApi.Services.Interfaces
{
    public interface ISocietyService
    {
        Task<ApiResponse<SocietyDto>> CreateSocietyAsync(SocietyCreateUpdateDto dto);
        Task<ApiResponse<SocietyDto>> GetSocietyByIdAsync(int id);
        Task<ApiResponse<List<SocietyDto>>> GetAllSocietiesAsync();
        Task<ApiResponse<SocietyDto>> UpdateSocietyAsync(int id, SocietyCreateUpdateDto dto);
        Task<ApiResponse<bool>> DeleteSocietyAsync(int id);
    }
}

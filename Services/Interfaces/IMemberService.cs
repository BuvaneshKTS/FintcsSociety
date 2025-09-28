using FintcsApi.DTOs;
using FintcsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FintcsApi.Services.Interfaces
{
    public interface IMemberService
    {
        // Create a new member for a specific society
        Task<ApiResponse<bool>> CreateMemberAsync(int societyId, MemberCreateUpdateDto dto);

        // Get a member by its Id
        Task<ApiResponse<MemberDto>> GetMemberByIdAsync(int memberId);

        // Get all members of a specific society
        Task<ApiResponse<List<MemberDto>>> GetAllMembersBySocietyAsync(int societyId);

        // Update an existing member by Id
        Task<ApiResponse<bool>> UpdateMemberAsync(int memberId, MemberUpdateDto dto);

        // Delete a member by Id
        Task<ApiResponse<bool>> DeleteMemberAsync(int memberId);
    }
}

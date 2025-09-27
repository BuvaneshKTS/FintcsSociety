using FintcsApi.DTOs;
using FintcsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FintcsApi.Services.Interfaces
{
    public interface IMemberService
    {
        // Create a new member for a specific society
        Task<(bool Success, string Message, MemberDto? Data)> CreateMemberAsync(int societyId, MemberCreateUpdateDto dto);

        // Get a member by its Id
        Task<(bool Success, string Message, MemberDto? Data)> GetMemberByIdAsync(int memberId);

        // Get all members of a specific society
        Task<(bool Success, string Message, List<MemberDto> Data)> GetAllMembersBySocietyAsync(int societyId);

        // Update an existing member by Id
        Task<(bool Success, string Message)> UpdateMemberAsync(int memberId, MemberCreateUpdateDto dto);

        // Delete a member by Id
        Task<(bool Success, string Message)> DeleteMemberAsync(int memberId);
    }
}

using FintcsApi.DTOs;
using FintcsApi.Models;

namespace FintcsApi.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponse<UserResponseDto>> CreateUserAsync(RegisterDto registerDto);
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto);
    Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id);
    Task<ApiResponse<List<UserResponseDto>>> GetAllUsersAsync();
    Task<ApiResponse<UserResponseDto>> UpdateUserAsync(int id, UserDetailsDto userDetailsDto);
    Task<ApiResponse<UserResponseDto>> UpdateUserRoleAsync(int id, string role);
    Task<ApiResponse<bool>> DeleteUserAsync(int id);
    Task<ApiResponse<UserResponseDto>> SetupAdminAsync(RegisterDto registerDto);
    Task<bool> IsAdminExistsAsync();
}
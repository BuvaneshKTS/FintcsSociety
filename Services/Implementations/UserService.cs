using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Services.Implementations;

public enum UserRole
{
    Admin,
    User
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;

    public UserService(AppDbContext context, IConfiguration configuration, ILogger<UserService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApiResponse<UserResponseDto>> CreateUserAsync(RegisterDto registerDto)
    {
        try
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                return ApiResponse<UserResponseDto>.ErrorResponse("Username already exists");

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return ApiResponse<UserResponseDto>.ErrorResponse("Email already exists");

            int saltRounds = int.Parse(_configuration["Security:BCryptWorkFactor"] ?? "12");

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, saltRounds),
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                Name = registerDto.Name,
                AddressOffice = registerDto.AddressOffice,
                AddressResidential = registerDto.AddressResidential,
                Designation = registerDto.Designation,
                PhoneOffice = registerDto.PhoneOffice,
                PhoneResidential = registerDto.PhoneResidential,
                Mobile = registerDto.Mobile,
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return ApiResponse<UserResponseDto>.SuccessResponse(MapToUserResponse(user), "User created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return ApiResponse<UserResponseDto>.ErrorResponse("An error occurred while creating the user");
        }
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return ApiResponse<LoginResponseDto>.ErrorResponse("Invalid username or password");

            var token = GenerateJwtToken(user);
            var expiresHours = int.Parse(_configuration["Jwt:ExpiresHours"] ?? "2");

            return ApiResponse<LoginResponseDto>.SuccessResponse(new LoginResponseDto
            {
                Token = token,
                Expires = DateTime.UtcNow.AddHours(expiresHours),
                User = MapToUserResponse(user)
            }, "Login successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return ApiResponse<LoginResponseDto>.ErrorResponse("An error occurred during login");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return ApiResponse<UserResponseDto>.ErrorResponse("User not found");

            return ApiResponse<UserResponseDto>.SuccessResponse(MapToUserResponse(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by Id");
            return ApiResponse<UserResponseDto>.ErrorResponse("An error occurred while retrieving the user");
        }
    }

    public async Task<ApiResponse<List<UserResponseDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return ApiResponse<List<UserResponseDto>>.SuccessResponse(users.Select(MapToUserResponse).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            return ApiResponse<List<UserResponseDto>>.ErrorResponse("An error occurred while retrieving users");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> UpdateUserAsync(int id, UserDetailsDto dto)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return ApiResponse<UserResponseDto>.ErrorResponse("User not found");

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                return ApiResponse<UserResponseDto>.ErrorResponse("Email already exists");

            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Name = dto.Name;
            user.AddressOffice = dto.AddressOffice;
            user.AddressResidential = dto.AddressResidential;
            user.Designation = dto.Designation;
            user.PhoneOffice = dto.PhoneOffice;
            user.PhoneResidential = dto.PhoneResidential;
            user.Mobile = dto.Mobile;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResponse<UserResponseDto>.SuccessResponse(MapToUserResponse(user), "User updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user");
            return ApiResponse<UserResponseDto>.ErrorResponse("An error occurred while updating the user");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> UpdateUserRoleAsync(int id, string role)
    {
        try
        {
            if (!Enum.TryParse<UserRole>(role, true, out var newRole))
                return ApiResponse<UserResponseDto>.ErrorResponse("Invalid role");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return ApiResponse<UserResponseDto>.ErrorResponse("User not found");

            user.Role = newRole.ToString();
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResponse<UserResponseDto>.SuccessResponse(MapToUserResponse(user), "User role updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user role");
            return ApiResponse<UserResponseDto>.ErrorResponse("An error occurred while updating user role");
        }
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return ApiResponse<bool>.ErrorResponse("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "User deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user");
            return ApiResponse<bool>.ErrorResponse("An error occurred while deleting the user");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> SetupAdminAsync(RegisterDto dto)
    {
        try
        {
            if (await IsAdminExistsAsync())
                return ApiResponse<UserResponseDto>.ErrorResponse("Admin user already exists");

            dto.Role = UserRole.Admin.ToString();
            return await CreateUserAsync(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting up admin");
            return ApiResponse<UserResponseDto>.ErrorResponse("An error occurred while setting up admin");
        }
    }

    public async Task<bool> IsAdminExistsAsync()
    {
        return await _context.Users.AnyAsync(u => u.Role == UserRole.Admin.ToString());
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var expiresHours = int.Parse(_configuration["Jwt:ExpiresHours"] ?? "2");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(expiresHours),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
    }

    private static UserResponseDto MapToUserResponse(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Name = user.Name,
            AddressOffice = user.AddressOffice,
            AddressResidential = user.AddressResidential,
            Designation = user.Designation,
            PhoneOffice = user.PhoneOffice,
            PhoneResidential = user.PhoneResidential,
            Mobile = user.Mobile,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}

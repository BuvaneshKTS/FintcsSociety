using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.Services.Interfaces;
using FintcsApi.DTOs;
namespace FintcsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        // Get current user ID from token
        var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out int currentUserId))
        {
            return Unauthorized("Invalid token");
        }

        var result = await _userService.GetUserByIdAsync(currentUserId);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return NotFound(result);
    }

    [HttpPut("{id}/role")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateRoleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(request.Role))
        {
            return BadRequest("Role is required");
        }

        if (request.Role != "admin" && request.Role != "user")
        {
            return BadRequest("Role must be either 'admin' or 'user'");
        }

        var result = await _userService.UpdateUserRoleAsync(id, request.Role);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }
}

public class UpdateRoleRequest
{
    public string Role { get; set; } = string.Empty;
}
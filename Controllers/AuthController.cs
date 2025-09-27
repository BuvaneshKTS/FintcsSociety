using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("setup-admin")]
    public async Task<IActionResult> SetupAdmin([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.SetupAdminAsync(registerDto);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPost("register")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.CreateUserAsync(registerDto);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.LoginAsync(loginDto);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return Unauthorized(result);
    }

    [HttpPut("update/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDetailsDto userDetailsDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Get current user ID from token
        var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out int currentUserId))
        {
            return Unauthorized("Invalid token");
        }

        // Check if user is admin or updating their own profile
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (userRole != "admin" && currentUserId != id)
        {
            return Forbid("You can only update your own profile");
        }

        var result = await _userService.UpdateUserAsync(id, userDetailsDto);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }
}
// DTOs/LoginResponseDto.cs
namespace FintcsApi.DTOs;
using System.ComponentModel.DataAnnotations;


public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public UserResponseDto User { get; set; } = new();
}
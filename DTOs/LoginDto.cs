// DTOs/LoginDto.cs
using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs;

public class LoginDto
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;
}
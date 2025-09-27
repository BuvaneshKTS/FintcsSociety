using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Name { get; set; }
    public string? AddressOffice { get; set; }
    public string? AddressResidential { get; set; }
    public string? Designation { get; set; }
    public string? PhoneOffice { get; set; }
    public string? PhoneResidential { get; set; }
    public string? Mobile { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
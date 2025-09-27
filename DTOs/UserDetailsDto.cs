using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs;

public class UserDetailsDto
{
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string? Phone { get; set; }
    
    [StringLength(255)]
    public string? Name { get; set; }
    
    [StringLength(500)]
    public string? AddressOffice { get; set; }
    
    [StringLength(500)]
    public string? AddressResidential { get; set; }
    
    [StringLength(100)]
    public string? Designation { get; set; }
    
    [StringLength(20)]
    public string? PhoneOffice { get; set; }
    
    [StringLength(20)]
    public string? PhoneResidential { get; set; }
    
    [StringLength(20)]
    public string? Mobile { get; set; }
}
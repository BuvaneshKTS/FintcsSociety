using System.ComponentModel.DataAnnotations;

namespace FintcsApi.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
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
    
    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "user";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
}
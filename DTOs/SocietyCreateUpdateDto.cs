using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs;

public class SocietyCreateUpdateDto
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(20)]
    public string? Fax { get; set; }

    [EmailAddress]
    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? Website { get; set; }

    [StringLength(100)]
    public string? RegistrationNumber { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? ChequeBounceCharge { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FintcsApi.Models;

[Index(nameof(SocietyId), nameof(Name), IsUnique = true)]
public class LoanType
{
    [Key]
    public int LoanTypeId { get; set; }  // Changed from Guid to int

    [Required]
    public int SocietyId { get; set; }   // FK changed to int

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, 100)]
    public decimal InterestPercent { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal LimitAmount { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal CompulsoryDeposit { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal OptionalDeposit { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ShareAmount { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int XTimes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("SocietyId")]
    public Society? Society { get; set; }  // Navigation
}

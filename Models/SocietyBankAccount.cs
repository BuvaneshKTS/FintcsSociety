using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FintcsApi.Models;

[Index(nameof(SocietyId), nameof(BankName), nameof(AccountNumber), IsUnique = true)]
public class SocietyBankAccount
{
    [Key]
    public int Id { get; set; }  // Changed from Guid to int

    [Required]
    public int SocietyId { get; set; }   // FK changed to int

    [Required]
    [StringLength(255)]
    public string BankName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string AccountNumber { get; set; } = string.Empty;

    [StringLength(20)]
    public string? IFSC { get; set; }

    [StringLength(255)]
    public string? Branch { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("SocietyId")]
    public Society? Society { get; set; }  // Navigation
}

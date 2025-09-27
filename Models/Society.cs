using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FintcsApi.Models;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Phone), IsUnique = true)]
[Index(nameof(Fax), IsUnique = true)]
[Index(nameof(Address), IsUnique = true)]
[Index(nameof(Website), IsUnique = true)]
[Index(nameof(RegistrationNumber), IsUnique = true)]
public class Society
{
    [Key]
    public int Id { get; set; }  // Changed from Guid to int

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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<LoanType>? LoanTypes { get; set; }
    public ICollection<SocietyBankAccount>? BankAccounts { get; set; }
    public ICollection<Member>? Members { get; set; }
}

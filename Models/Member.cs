using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FintcsApi.Models;

public class Member
{
    [Key]
    public int Id { get; set; }   // EF Core auto-generates int Id

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string FHName { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string Mobile { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Status { get; set; } = "Active";

    // Optional fields
    public string OfficeAddress { get; set; } = "";
    public string City { get; set; } = "";
    public string PhoneOffice { get; set; } = "";
    public string Branch { get; set; } = "";
    public string PhoneRes { get; set; } = "";
    public string Designation { get; set; } = "";
    public string ResidenceAddress { get; set; } = "";
    public DateTime? DOB { get; set; }
    public DateTime? DOJSociety { get; set; }
    public DateTime? DOR { get; set; }
    public string Nominee { get; set; } = "";
    public string NomineeRelation { get; set; } = "";
    public decimal CdAmount { get; set; } = 0;
    public string Email2 { get; set; } = "";
    public string Mobile2 { get; set; } = "";
    public string Pincode { get; set; } = "";
    public string BankName { get; set; } = "";
    public string AccountNumber { get; set; } = "";
    public string PayableAt { get; set; } = "";
    public decimal Share { get; set; } = 0;

    // Audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationship
    [Required]
    public int SocietyId { get; set; }

    [ForeignKey("SocietyId")]
    public Society? Society { get; set; }
}

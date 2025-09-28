using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs;

public class MemberCreateUpdateDto
{
    [Required]
    public int SocietyId { get; set; }

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

    public string Status { get; set; } = "Active";

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
}

public class MemberDto : MemberCreateUpdateDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class MemberUpdateDto
{
    public int? SocietyId { get; set; }

    public string? Name { get; set; }

    public string? FHName { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public string? Status { get; set; }

    public string? OfficeAddress { get; set; }
    public string? City { get; set; }
    public string? PhoneOffice { get; set; }
    public string? Branch { get; set; }
    public string? PhoneRes { get; set; }
    public string? Designation { get; set; }
    public string? ResidenceAddress { get; set; }
    public DateTime? DOB { get; set; }
    public DateTime? DOJSociety { get; set; }
    public DateTime? DOR { get; set; }
    public string? Nominee { get; set; }
    public string? NomineeRelation { get; set; }
    public decimal? CdAmount { get; set; }
    public string? Email2 { get; set; }
    public string? Mobile2 { get; set; }
    public string? Pincode { get; set; }
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? PayableAt { get; set; }
    public decimal? Share { get; set; }
}

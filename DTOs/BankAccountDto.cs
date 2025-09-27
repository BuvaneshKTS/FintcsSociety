// DTOs/BankAccountDto.cs
using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs
{
    public class BankAccountCreateUpdateDto
    {
        public int? Id { get; set; }

        [Required]
        public int SocietyId { get; set; }

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
    }

    public class BankAccountDto : BankAccountCreateUpdateDto
    {
        public new int Id { get; set; }
    }
}

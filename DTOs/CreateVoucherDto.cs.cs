using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs
{
    // DTO for creating a voucher
    public class CreateVoucherDto
    {  // Links to the main transaction

        // [Required]
        // public int PayId { get; set; }                 // Payment reference

        public int? LedgerAccountId { get; set; }

        public int ParticularId { get; set; }

        [Required]
        public int SocietyId { get; set; }

        [Required]
        [StringLength(50)]
        public string VoucherType { get; set; } = "Journal"; // e.g., Receipt, Payment, Contra

        public DateTime VoucherDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Narration { get; set; }

        public int? MemberId { get; set; }
        public int? LoanId { get; set; }

        [Required]
        public decimal Amount { get; set; }
        public decimal BankId { get; set; }

        public string? ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }

        // Voucher entries
        // public List<VoucherEntryDto> Entries { get; set; } = new();
    }

    // DTO for each ledger entry in the voucher
    public class VoucherEntryDto
    {
        [Required]
        public int LedgerAccountId { get; set; }

        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int PayId { get; set; }

        [Required]
        public int SocietyId { get; set; }
    }

    // DTO for returning voucher info (optional)
    public class VoucherDto
    {
        public int VoucherId { get; set; }
        public int PayId { get; set; }
        public int ParticularId { get; set; }
        public int SocietyId { get; set; }
        public string VoucherType { get; set; } = null!;
        public DateTime VoucherDate { get; set; }
        public string? Narration { get; set; }
        public int? MemberId { get; set; }
        public int? LoanId { get; set; }
        public decimal Amount { get; set; }
        public decimal BankId { get; set; }
        public string? ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }

        // public List<VoucherEntryDto> Entries { get; set; } = new();
    }
}

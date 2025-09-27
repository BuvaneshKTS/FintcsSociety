using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FintcsApi.Models
{
    public class Voucher
    {
        [Key]
        public int VoucherId { get; set; } // changed from Guid

        [Required]
        public int LedgerTransactionId { get; set; }

        [ForeignKey("LedgerTransactionId")]
        public LedgerTransaction LedgerTransaction { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string VoucherNumber { get; set; } = string.Empty; // Auto-generated

        [Required]
        [StringLength(50)]
        public string VoucherType { get; set; } = "Journal"; // e.g., Receipt, Payment, Contra

        public DateTime VoucherDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string Narration { get; set; } = string.Empty;

        // 🔹 Updated IDs
        public int? MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member? Member { get; set; }

        public int? LoanId { get; set; }

        [ForeignKey("LoanId")]
        public Loan? Loan { get; set; }

        // 🔹 Amount field to track transaction value
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

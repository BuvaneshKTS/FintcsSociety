using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FintcsApi.Models
{
    public class LedgerTransaction
    {
        [Key]
        public int LedgerTransactionId { get; set; }

        [Required]
        public int LedgerAccountId { get; set; }

        [ForeignKey("LedgerAccountId")]
        public LedgerAccount LedgerAccount { get; set; } = null!;

        [Required]
        public int ParticularId { get; set; }

        [Required]
        public int PayId { get; set; }

        public int? MemberId { get; set; }
        public int? LoanId { get; set; }

        [Required]
        public int SocietyId { get; set; }

        public int? BankId { get; set; }

        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? VoucherId { get; set; }

        [ForeignKey("VoucherId")]
        public Voucher? Voucher { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace FintcsApi.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        [Required]
        public int SocietyId { get; set; }
        public Society? Society { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member? Member { get; set; }

        [Required]
        public int LoanTypeId { get; set; }
        public LoanType? LoanType { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        public decimal LoanAmount { get; set; }
        public decimal PreviousLoan { get; set; }
        public int Installments { get; set; }
        public string? Purpose { get; set; }
        public string? AuthorizedBy { get; set; }
        public string PaymentMode { get; set; } = "Cash";

        public int Bank { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Status { get; set; } = "Active";

        public decimal NetLoan { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal NewLoanShare { get; set; }
        public decimal PayAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

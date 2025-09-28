using System;
using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs
{
    public class LoanDto
    {
        public int LoanId { get; set; }

        [Required]
        public int SocietyId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int LoanTypeId { get; set; }

        // Read-only fields
        public string LoanTypeName { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public decimal LoanAmount { get; set; }

        [Required]
        public int Installments { get; set; }

        public string? Purpose { get; set; }
        public string? AuthorizedBy { get; set; }

        [Required]
        public string PaymentMode { get; set; } = "Cash";

        public string Status { get; set; } = "Active";

        public string? Bank { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }

        // Financials
        public decimal NetLoan { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal NewLoanShare { get; set; }
        public decimal PayAmount { get; set; }
        public decimal PreviousLoan { get; set; }
    }

    public class LoanCreateUpdateDto
    {
        [Required]
        public int SocietyId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int LoanTypeId { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public decimal LoanAmount { get; set; }

        [Required]
        public int Installments { get; set; }

        public string? Purpose { get; set; }
        public string? AuthorizedBy { get; set; }

        [Required]
        public string PaymentMode { get; set; } = "Cash";

        public string? Bank { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Status { get; set; } = "Active";

        // Financials
        [Required]
        public decimal NetLoan { get; set; }

        [Required]
        public decimal InstallmentAmount { get; set; }

        public decimal NewLoanShare { get; set; }
        public decimal PayAmount { get; set; }
        public decimal PreviousLoan { get; set; }
    }
}

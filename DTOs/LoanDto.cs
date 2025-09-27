using System;
using System.ComponentModel.DataAnnotations;


namespace FintcsApi.DTOs
{
    public class LoanDto
    {
        public int LoanId { get; set; }
        public int SocietyId { get; set; }
        public int MemberId { get; set; }
        public int LoanTypeId { get; set; }
        public string LoanTypeName { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;

        public DateTime LoanDate { get; set; }
        public decimal LoanAmount { get; set; }
        public int Installments { get; set; }
        public string? Purpose { get; set; }
        public string? AuthorizedBy { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";

        public decimal NetLoan { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal NewLoanShare { get; set; }
        public decimal PayAmount { get; set; }
        public decimal PreviousLoan { get; set; } // reference if needed
    }
}

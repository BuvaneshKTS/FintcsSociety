using System;
using System.ComponentModel.DataAnnotations;


namespace FintcsApi.DTOs
{
    public class LoanCreateUpdateDto
    {
        public int SocietyId { get; set; }
        public int MemberId { get; set; }
        public int LoanTypeId { get; set; }
        public DateTime LoanDate { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal PreviousLoan { get; set; } = 0;
        public int Installments { get; set; }
        public string? Purpose { get; set; }
        public string? AuthorizedBy { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string? Bank { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Status { get; set; } = "Pending";

        // Derived/extra amounts
        public decimal NetLoan { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal NewLoanShare { get; set; }
        public decimal PayAmount { get; set; }
    }
}

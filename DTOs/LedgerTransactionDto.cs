using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs
{
    public class LedgerTransactionDto
    {
        public int LedgerAccountId { get; set; }
        public int? MemberId { get; set; }
        public int? LoanId { get; set; }

        public int ParticularId { get; set; }
        public int PayId { get; set; }

        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;

        public int SocietyId { get; set; }
        public int? BankId { get; set; }
        public int VoucherId { get; set; }

        public string Description { get; set; } = "";
    }
}

using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs
{
    public class LedgerCreateDto
    {
        public int? MemberId { get; set; }
        public int SocietyId { get; set; }
        public string AccountName { get; set; } = null!;
        public string AccountType { get; set; } = "General"; // e.g., Share, Loan, Cash, Bank
        public decimal InitialBalance { get; set; } = 0;
    }

    public class LedgerAccountDto
    {
        public int LedgerAccountId { get; set; }
        public int? MemberId { get; set; }
        public int SocietyId { get; set; }
        public string AccountName { get; set; } = null!;
        public string AccountType { get; set; } = "General";
        public decimal Balance { get; set; }
    }
}

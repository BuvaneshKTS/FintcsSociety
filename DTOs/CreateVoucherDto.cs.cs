using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FintcsApi.DTOs
{
    public class CreateVoucherDto
    {
        public string VoucherType { get; set; } = null!;
        public int? MemberId { get; set; }   // changed from Guid
        public int? LoanId { get; set; }     // changed from Guid
        public string? Narration { get; set; }

        public List<VoucherEntryDto> Entries { get; set; } = new();
    }

    public class VoucherEntryDto
    {
        public int LedgerAccountId { get; set; }  // changed from Guid
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? Description { get; set; }
    }
}

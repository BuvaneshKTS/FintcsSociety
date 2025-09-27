using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs;

public class SocietyDto : SocietyCreateUpdateDto
{
    public int Id { get; set; }

    public List<BankAccountDto>? BankAccounts { get; set; }
    public List<LoanTypeDto>? LoanTypes { get; set; }
}

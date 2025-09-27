// Services/Interfaces/ILedgerService.cs
using FintcsApi.DTOs;
using System.Threading.Tasks;

namespace FintcsApi.Services.Interfaces
{
    public interface ILedgerService
    {
        Task RecordTransactionAsync(LedgerTransactionDto dto);
        Task CreateDefaultLedgersForMemberAsync(int memberId);
        Task CreateOtherLedgerAsync(int? memberId, string accountName, decimal initialBalance = 0);
        Task RecordOtherLedgerTransactionAsync(LedgerTransactionDto dto);
    }
}

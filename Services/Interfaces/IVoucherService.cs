using System.Collections.Generic;
using System.Threading.Tasks;
using FintcsApi.Models;
using FintcsApi.DTOs;

namespace FintcsApi.Services.Interfaces
{
    public interface IVoucherService
    {
        Task<Voucher> CreateVoucherAsync(CreateVoucherDto dto);

        // New methods
        Task<IEnumerable<Voucher>> GetAllVouchersAsync();
        Task<Voucher?> GetVoucherByIdAsync(int id);
    }
}

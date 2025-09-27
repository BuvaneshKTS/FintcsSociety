using System.Threading.Tasks;
using FintcsApi.DTOs;
using FintcsApi.Models;

namespace FintcsApi.Services.Interfaces
{
    public interface IVoucherService
    {
        Task<Voucher> CreateVoucherAsync(CreateVoucherDto dto);
    }
}

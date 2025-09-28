using System;
using System.Threading.Tasks;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FintcsApi.Services.Implementations
{
    public class VoucherService : IVoucherService
    {
        private readonly AppDbContext _context;

        public VoucherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Voucher> CreateVoucherAsync(CreateVoucherDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            int nextPayId = 1;
                var lastVoucher = await _context.Vouchers
                    .OrderByDescending(v => v.PayId)
                    .FirstOrDefaultAsync();
                if (lastVoucher != null)
                    nextPayId = lastVoucher.PayId + 1;

            // Create voucher
            var voucher = new Voucher
            {
                VoucherType = dto.VoucherType,
                MemberId = dto.MemberId,
                LoanId = dto.LoanId,
                Narration = dto.Narration,
                Amount = dto.Amount,
                BankId = dto.BankId,
                ChecqueNumber = dto.ChequeNumber,
                ChecqueDate = dto.ChequeDate,
                VoucherDate = dto.VoucherDate,
                PayId = nextPayId,
                ParticularId = dto.ParticularId,
                SocietyId = dto.SocietyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();



            return voucher;
        }
    }
}

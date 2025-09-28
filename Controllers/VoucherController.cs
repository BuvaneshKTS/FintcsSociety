using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        /// <summary>
        /// Create a new voucher with ledger transactions
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<Voucher>.ErrorResponse(
                    "Invalid input", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                ));
            }

            try
            {
                var voucher = await _voucherService.CreateVoucherAsync(dto);

                return CreatedAtAction(
                    nameof(CreateVoucher),
                    new { id = voucher.VoucherId },
                    ApiResponse<Voucher>.SuccessResponse(voucher, "Voucher created successfully")
                );
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<Voucher>.ErrorResponse($"Error creating voucher: {ex.Message}"));
            }
        }
    }
}

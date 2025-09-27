using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FintcsApi.DTOs;
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid input", errors = ModelState });

            try
            {
                var voucher = await _voucherService.CreateVoucherAsync(dto);

                return Ok(new
                {
                    success = true,
                    message = "Voucher created successfully",
                    data = voucher
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"Error creating voucher: {ex.Message}"
                });
            }
        }
    }
}

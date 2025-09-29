using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using FintcsApi.DTOs;

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

        // Existing POST /api/voucher/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<Voucher>.ErrorResponse(
                        "Invalid input",
                        ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    )
                );
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
                return BadRequest(
                    ApiResponse<Voucher>.ErrorResponse($"Error creating voucher: {ex.Message}")
                );
            }
        }


        /// <summary>
        /// Get all vouchers
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllVouchersAsync()
        {
            var vouchers = await _voucherService.GetAllVouchersAsync();
            return Ok(ApiResponse<IEnumerable<Voucher>>.SuccessResponse(vouchers, "Vouchers retrieved successfully"));
        }

        /// <summary>
        /// Get voucher by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherById(int id)
        {
            var voucher = await _voucherService.GetVoucherByIdAsync(id);
            if (voucher == null)
                return NotFound(ApiResponse<Voucher>.ErrorResponse("Voucher not found"));

            return Ok(ApiResponse<Voucher>.SuccessResponse(voucher, "Voucher retrieved successfully"));
        }
    }
}

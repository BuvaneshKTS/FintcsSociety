// using System;
// using System.Threading.Tasks;
// using FintcsApi.DTOs;
// using FintcsApi.Services.Interfaces;
// using Microsoft.AspNetCore.Mvc;

// namespace FintcsApi.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class LedgerController : ControllerBase
//     {
//         private readonly ILedgerService _ledgerService;

//         public LedgerController(ILedgerService ledgerService)
//         {
//             _ledgerService = ledgerService;
//         }

//         [HttpPost("member/{memberId}/create-ledgers")]
//         public async Task<IActionResult> CreateMemberLedgers(Guid memberId)
//         {
//             await _ledgerService.CreateDefaultLedgersForMemberAsync(memberId);
//             return Ok(new { success = true, message = "Ledgers created for member." });
//         }

//         [HttpPost("transaction")]
//         public async Task<IActionResult> RecordTransaction([FromBody] LedgerTransactionDto dto)
//         {
//             await _ledgerService.RecordTransactionAsync(dto);
//             return Ok(new { success = true, message = "Transaction recorded." });
//         }

//         [HttpPost("create-other-ledger")]
//         public async Task<IActionResult> CreateOtherLedger([FromBody] LedgerCreateDto dto)
//         {
//             await _ledgerService.CreateOtherLedgerAsync(dto.MemberId, dto.AccountName, dto.InitialBalance);
//             return Ok("Ledger created successfully");
//         }

//         [HttpPost("other-ledger-transaction")]
//         public async Task<IActionResult> OtherLedgerTransaction([FromBody] LedgerTransactionDto dto)
//         {
//             await _ledgerService.RecordOtherLedgerTransactionAsync(dto);
//             return Ok("Transaction recorded successfully");
//         }
//     }
// }


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;
using System.Threading.Tasks;

namespace FintcsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LedgerController : ControllerBase
    {
        private readonly ILedgerService _ledgerService;

        public LedgerController(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }

        [HttpPost("other-ledger-transaction")]
        [Authorize]
        public async Task<IActionResult> RecordOtherLedgerTransactionAsync([FromBody] LedgerTransactionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid input", errors = ModelState });

            await _ledgerService.RecordOtherLedgerTransactionAsync(dto);
            return Ok(new { success = true, message = "Transaction recorded successfully" });
        }

        [HttpPost("create-other-ledger")]
        [Authorize]
        public async Task<IActionResult> CreateOtherLedger([FromBody] LedgerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid input", errors = ModelState });

            await _ledgerService.CreateOtherLedgerAsync(dto.MemberId, dto.AccountName, dto.InitialBalance);
            return Ok(new { success = true, message = "Ledger created successfully" });
        }

        [HttpGet("all")]
        [Authorize] // Any authorized user
        public async Task<IActionResult> GetAllLedgerAccounts()
        {
            var ledgers = await _ledgerService.GetAllLedgerAccountsAsync();
            return Ok(new { success = true, data = ledgers });
        }

        [HttpGet("{id}")]
        [Authorize] // Any authorized user
        public async Task<IActionResult> GetLedgerAccountById(int id)
        {
            var ledger = await _ledgerService.GetLedgerAccountByIdAsync(id);
            if (ledger == null)
                return NotFound(new { success = false, message = "Ledger not found" });

            return Ok(new { success = true, data = ledger });
        }

    }
}

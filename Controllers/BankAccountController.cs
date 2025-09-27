
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBankAccountById(int id) // Use Guid because DTO uses Guid
        {
            var result = await _bankAccountService.GetBankAccountByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBankAccountsBySocietyAsync(int societyId)
        {
            var result = await _bankAccountService.GetAllBankAccountsBySocietyAsync(societyId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBankAccount([FromBody] BankAccountCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bankAccountService.CreateBankAccountAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBankAccount(int id, [FromBody] BankAccountCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bankAccountService.UpdateBankAccountAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBankAccount(int id)
        {
            var result = await _bankAccountService.DeleteBankAccountAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}

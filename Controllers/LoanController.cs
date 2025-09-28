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
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanById(int id)
        {
            var result = await _loanService.GetLoanByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("society")]
        public async Task<IActionResult> GetLoansBySociety([FromQuery] int societyId)
        {
            var result = await _loanService.GetLoansBySocietyAsync(societyId);
            return Ok(result);
        }

        [HttpGet("member")]
        public async Task<IActionResult> GetLoansByMember([FromQuery] int memberId)
        {
            var result = await _loanService.GetLoansByMemberAsync(memberId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLoan([FromBody] LoanCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loanService.CreateLoanAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loanService.UpdateLoanAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var result = await _loanService.DeleteLoanAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}

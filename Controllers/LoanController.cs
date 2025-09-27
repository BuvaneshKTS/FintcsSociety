// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using FintcsApi.DTOs;
// using FintcsApi.Services.Interfaces;

// namespace FintcsApi.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class LoanController : ControllerBase
// {
//     private readonly ILoanService _loanService;

//     public LoanController(ILoanService loanService)
//     {
//         _loanService = loanService;
//     }

//     [HttpPost]
//     public async Task<IActionResult> CreateLoan([FromBody] LoanCreateUpdateDto dto)
//     {
//         var (success, message, data) = await _loanService.CreateLoanAsync(dto);
//         if (success) return Ok(new { success, message, data });
//         return BadRequest(new { success, message });
//     }

//     [HttpGet("society/{societyId}")]
//     public async Task<IActionResult> GetLoansBySociety(Guid societyId)
//     {
//         var (success, message, data) = await _loanService.GetLoansBySocietyAsync(societyId);
//         if (success) return Ok(new { success, message, data });
//         return NotFound(new { success, message });
//     }

//     [HttpGet("{loanId}")]
//     public async Task<IActionResult> GetLoan(Guid loanId)
//     {
//         var (success, message, data) = await _loanService.GetLoanByIdAsync(loanId);
//         if (success) return Ok(new { success, message, data });
//         return NotFound(new { success, message });
//     }

//     [HttpPut("{loanId}")]
//     public async Task<IActionResult> UpdateLoan(Guid loanId, [FromBody] LoanCreateUpdateDto dto)
//     {
//         var (success, message) = await _loanService.UpdateLoanAsync(loanId, dto);
//         if (success) return Ok(new { success, message });
//         return BadRequest(new { success, message });
//     }

//     [HttpDelete("{loanId}")]
//     public async Task<IActionResult> DeleteLoan(Guid loanId)
//     {
//         var (success, message) = await _loanService.DeleteLoanAsync(loanId);
//         if (success) return Ok(new { success, message });
//         return BadRequest(new { success, message });
//     }

//     [HttpGet("member/{memberId}")]
//     public async Task<IActionResult> GetLoansByMember(Guid memberId)
//     {
//         var (success, message, data) = await _loanService.GetLoansByMemberAsync(memberId);
//         if (success) return Ok(new { success, message, data });
//         return NotFound(new { success, message });
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
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoansBySocietyAsync(int societyId)
        {
            var result = await _loanService.GetLoansBySocietyAsync(societyId);
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

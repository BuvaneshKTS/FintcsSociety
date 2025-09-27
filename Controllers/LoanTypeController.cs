

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
    public class LoanTypeController : ControllerBase
    {
        private readonly ILoanTypeService _loanTypeService;

        public LoanTypeController(ILoanTypeService loanTypeService)
        {
            _loanTypeService = loanTypeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanTypeById(int id)
        {
            var result = await _loanTypeService.GetLoanTypeByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLoanTypesBySocietyAsync( int societyId)
        {
            var result = await _loanTypeService.GetAllLoanTypesBySocietyAsync(societyId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLoanTypeAsync([FromBody] LoanTypeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loanTypeService.CreateLoanTypeAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLoanType(int id, [FromBody] LoanTypeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loanTypeService.UpdateLoanTypeAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLoanType(int id)
        {
            var result = await _loanTypeService.DeleteLoanTypeAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}

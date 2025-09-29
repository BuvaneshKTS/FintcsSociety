using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;
using FintcsApi.Models;

namespace FintcsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DemandController : ControllerBase
    {
        private readonly IDemandService _demandService;

        public DemandController(IDemandService demandService)
        {
            _demandService = demandService;
        }

        // POST: api/demand/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateDemand([FromBody] DemandCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<bool>.ErrorResponse("Invalid input", ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

            var response = await _demandService.CreateDemandAsync(dto);

            if (!response.Success)
                return StatusCode(500, response); // return ApiResponse with errors

            return Ok(response); // return success ApiResponse
        }

        // GET: api/demand/{year}/{month}
        [HttpGet("{year:int}/{month:int}")]
        public async Task<IActionResult> GetDemandByMonthYear(int year, int month)
        {
            var response = await _demandService.GetDemandByMonthYearAsync(year, month);

            if (!response.Success)
                return NotFound(response); // return ApiResponse with message/errors

            return Ok(response); // return success ApiResponse
        }

        // DELETE: api/demand/{year}/{month}
        [HttpDelete("{year:int}/{month:int}")]
        public async Task<IActionResult> DeleteDemand(int year, int month)
        {
            var response = await _demandService.DeleteDemandAsync(year, month);

            if (!response.Success)
                return NotFound(response); // return ApiResponse with message/errors

            return Ok(response); // return success ApiResponse
        }
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SocietyController : ControllerBase
    {
        private readonly ISocietyService _societyService;

        public SocietyController(ISocietyService societyService)
        {
            _societyService = societyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSocietyById(int id)
        {
            var result = await _societyService.GetSocietyByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSocieties()
        {
            var result = await _societyService.GetAllSocietiesAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSocietyAsync([FromBody] SocietyCreateUpdateDto dto)
        {
            Console.WriteLine("Received request to create society...");

            // Step 1: Validate model
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model validation failed:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($" - {state.Key}: {error.ErrorMessage}");
                    }
                }
                return BadRequest(ModelState);
            }
            Console.WriteLine("Model validation passed.");

            // Step 2: Call service to create society
            Console.WriteLine("Calling SocietyService.CreateSocietyAsync...");
            var result = await _societyService.CreateSocietyAsync(dto);

            // Step 3: Log result
            if (result.Success)
            {
                Console.WriteLine($"Society created successfully with Id: {result.Data.Id}");
                return Ok(result);
            }
            else
            {
                Console.WriteLine($"Failed to create society: {result.Message}");
                return BadRequest(result);
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSociety(int id, [FromBody] SocietyCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _societyService.UpdateSocietyAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSociety(int id)
        {
            var result = await _societyService.DeleteSocietyAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}

// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using FintcsApi.DTOs;
// using FintcsApi.Services.Interfaces;

// namespace FintcsApi.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// [Authorize]
// public class MemberController : ControllerBase
// {
//     private readonly IMemberService _memberService;

//     public MemberController(IMemberService memberService)
//     {
//         _memberService = memberService;
//     }

//     [HttpPost("society/{societyId}")]
//     [Authorize(Roles = "admin")]
//     public async Task<IActionResult> CreateMember(Guid societyId, [FromBody] MemberCreateUpdateDto dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);

//         var (success, message, data) = await _memberService.CreateMemberAsync(societyId, dto);
//         return success ? Ok(new { success, message, data }) : BadRequest(new { success, message });
//     }

//     [HttpGet("{memberId}")]
//     public async Task<IActionResult> GetMember(Guid memberId)
//     {
//         var (success, message, data) = await _memberService.GetMemberByIdAsync(memberId);
//         return success ? Ok(new { success, message, data }) : NotFound(new { success, message });
//     }

//     [HttpGet("society/{societyId}")]
//     public async Task<IActionResult> GetAllMembersBySociety(Guid societyId)
//     {
//         var (success, message, data) = await _memberService.GetAllMembersBySocietyAsync(societyId);
//         return success ? Ok(new { success, message, data }) : NotFound(new { success, message });
//     }

//     [HttpPut("{memberId}")]
//     [Authorize(Roles = "admin")]
//     public async Task<IActionResult> UpdateMember(Guid memberId, [FromBody] MemberCreateUpdateDto dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);

//         var (success, message) = await _memberService.UpdateMemberAsync(memberId, dto);
//         return success ? Ok(new { success, message }) : BadRequest(new { success, message });
//     }

//     [HttpDelete("{memberId}")]
//     [Authorize(Roles = "admin")]
//     public async Task<IActionResult> DeleteMember(Guid memberId)
//     {
//         var (success, message) = await _memberService.DeleteMemberAsync(memberId);
//         return success ? Ok(new { success, message }) : BadRequest(new { success, message });
//     }
// }


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var result = await _memberService.GetMemberByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMembersBySocietyAsync(int societyId)
        {
            var result = await _memberService.GetAllMembersBySocietyAsync(societyId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateMemberAsync(int societyId, [FromBody] MemberCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _memberService.CreateMemberAsync(societyId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] MemberCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _memberService.UpdateMemberAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}

using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly ILogger<MemberController> _logger;

        public MemberController(IMemberService memberService, ILogger<MemberController> logger)
        {
            _memberService = memberService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var members = await _memberService.GetAsync();
                _logger.LogInformation("Successfully retrieved all members.");
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all members.");
                return StatusCode(500, "An error occurred while retrieving members.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var member = await _memberService.GetByIdAsync(id);
                if (member == null)
                {
                    _logger.LogWarning("Member with ID {MemberId} not found.", id);
                    return NotFound($"Member with ID {id} not found.");
                }
                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting member with ID {MemberId}.", id);
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberDto memberDto)
        {
            try
            {
                await _memberService.AddAsync(memberDto);
                _logger.LogInformation("Member created successfully: {@MemberDto}", memberDto);
                return CreatedAtAction(nameof(GetById), new { id = memberDto.Id }, memberDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating member.");
                return StatusCode(500, "An error occurred while creating the member.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MemberDto memberDto)
        {
            if (id != memberDto.Id)
            {
                _logger.LogWarning("Update attempt with mismatched IDs: {Id} vs {DtoId}", id, memberDto.Id);
                return BadRequest("ID mismatch.");
            }

            try
            {
                await _memberService.UpdateAsync(memberDto);
                _logger.LogInformation("Member with ID {MemberId} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating member with ID {MemberId}.", id);
                return StatusCode(500, "An error occurred while updating.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _memberService.DeleteAsync(id);
                _logger.LogInformation("Member with ID {MemberId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting member with ID {MemberId}.", id);
                return StatusCode(500, "An error occurred while deleting.");
            }
        }
    }
}

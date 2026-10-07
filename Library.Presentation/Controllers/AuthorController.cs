using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorService authorService, ILogger<AuthorController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("خطایی در دریافت لیست نویسندگان رخ داد.");
            try
            {
                var authors = await _authorService.GetAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطایی در دریافت لیست نویسندگان رخ داد.");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("خطایی در دریافت لیست نویسندگان رخ داد.");
            try
            {
                var author = await _authorService.GetByIdAsync(id);
                if (author == null)
                    return NotFound($"نویسنده‌ای با شناسه {id} یافت نشد.");

                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت نویسنده با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorDto authorDto)
        {
            _logger.LogInformation("خطایی در دریافت لیست نویسندگان رخ داد.");
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _authorService.AddAsync(authorDto);
                return CreatedAtAction(nameof(GetById), new { id = authorDto.Id }, authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد نویسنده جدید");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorDto authorDto)
        {
            _logger.LogInformation("خطایی در دریافت لیست نویسندگان رخ داد.");
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != authorDto.Id)
                    return BadRequest("ID mismatch");

                await _authorService.UpdateAsync(authorDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی نویسنده با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("خطایی در دریافت لیست نویسندگان رخ داد.");
            try
            {
                await _authorService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف نویسنده با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }
    }
}

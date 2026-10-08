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
            try
            {
                // لاگ کردن شروع عملیات (اختیاری - اگر دیتابیس خیلی شلوغ شد این را حذف کنید)
                _logger.LogInformation("درخواست دریافت همه نویسندگان.");

                var authors = await _authorService.GetAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                // لاگ کردن خطا (فقط در صورت وقوع خطا)
                _logger.LogError(ex, "خطا در دریافت لیست نویسندگان.");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var author = await _authorService.GetByIdAsync(id);
                if (author == null)
                {
                    _logger.LogWarning("نویسنده با شناسه {Id} یافت نشد.", id);
                    return NotFound($"نویسنده‌ای با شناسه {id} یافت نشد.");
                }

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
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("داده‌های ارسالی برای ایجاد نویسنده معتبر نیستند.");
                    return BadRequest(ModelState);
                }

                await _authorService.AddAsync(authorDto);
                _logger.LogInformation("نویسنده جدید با شناسه {Id} با موفقیت ایجاد شد.", authorDto.Id);

                return CreatedAtAction(nameof(GetById), new { id = authorDto.Id }, authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد نویسنده جدید.");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorDto authorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != authorDto.Id)
                {
                    _logger.LogWarning("تلاش برای آپدیت نویسنده با ID نامعتبر. ID درخواستی: {Id}, ID در بدنه: {DtoId}", id, authorDto.Id);
                    return BadRequest("ID mismatch");
                }

                await _authorService.UpdateAsync(authorDto);
                _logger.LogInformation("نویسنده با شناسه {Id} به‌روزرسانی شد.", id);

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
            try
            {
                await _authorService.DeleteAsync(id);
                _logger.LogInformation("نویسنده با شناسه {Id} حذف شد.", id);
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


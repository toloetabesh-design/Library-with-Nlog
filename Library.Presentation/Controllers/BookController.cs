using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("درخواست دریافت لیست تمام کتاب‌ها.");
                var books = await _bookService.GetAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت لیست تمام کتاب‌ها");
                return StatusCode(500, "خطای داخلی سرور در دریافت کتاب‌ها");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var book = await _bookService.GetByIdAsync(id);
                if (book == null)
                {
                    // استفاده از Warning به جای Error چون این یک خطای سیستمی نیست، بلکه عدم وجود داده است
                    _logger.LogWarning("کتاب با شناسه {Id} یافت نشد.", id);
                    return NotFound($"کتابی با شناسه {id} یافت نشد.");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت کتاب با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("داده‌های ارسالی برای ایجاد کتاب نامعتبر است.");
                    return BadRequest(ModelState);
                }

                await _bookService.AddAsync(bookDto);

                // لاگ کردن موفقیت عملیات
                _logger.LogInformation("کتاب جدید با شناسه {Id} با موفقیت ایجاد شد.", bookDto.Id);

                return CreatedAtAction(nameof(GetById), new { id = bookDto.Id }, bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد کتاب جدید.");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != bookDto.Id)
                {
                    _logger.LogWarning("تلاش برای آپدیت کتاب با ID نامعتبر. ID در مسیر: {Id}, ID در بدنه: {DtoId}", id, bookDto.Id);
                    return BadRequest("شناسه کتاب در مسیر و بدنه درخواست با هم مطابقت ندارند.");
                }

                await _bookService.UpdateAsync(bookDto);

                _logger.LogInformation("کتاب با شناسه {Id} با موفقیت به‌روزرسانی شد.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی کتاب با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookService.DeleteAsync(id);

                _logger.LogInformation("کتاب با شناسه {Id} حذف شد.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف کتاب با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }
    }
}


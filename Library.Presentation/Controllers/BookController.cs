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
                    return NotFound($"کتابی با شناسه {id} یافت نشد.");

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت کتاب با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 3. ایجاد یک کتاب جدید
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookDto bookDto)
        {
            try
            {
                // بررسی اعتبار داده‌های ورودی (Validation)
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _bookService.AddAsync(bookDto);

                // بازگشت وضعیت 201 Created و لینک به متد GetById برای مشاهده کتاب ساخته شده
                return CreatedAtAction(nameof(GetById), new { id = bookDto.Id }, bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد کتاب جدید");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 4. به‌روزرسانی اطلاعات یک کتاب موجود
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // جلوگیری از به‌روزرسانی کتابی که ID آن با ID موجود در Body متفاوت است
                if (id != bookDto.Id)
                    return BadRequest("شناسه کتاب در مسیر و بدنه درخواست با هم مطابقت ندارند.");

                await _bookService.UpdateAsync(bookDto);
                return NoContent(); // بازگشت وضعیت 204 No Content (موفقیت‌آمیز بدون محتوای بازگشتی)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی کتاب با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 5. حذف یک کتاب
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookService.DeleteAsync(id);
                return NoContent(); // بازگشت وضعیت 204
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف کتاب با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }
    }
}

using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // مسیر: api/borrowing
    public class BorrowingController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;
        private readonly ILogger<BorrowingController> _logger;

        public BorrowingController(IBorrowingService borrowingService, ILogger<BorrowingController> logger)
        {
            _borrowingService = borrowingService;
            _logger = logger;
        }

        // 1. دریافت تمام سوابق امانت
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var borrowings = await _borrowingService.GetAsync();
                return Ok(borrowings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت لیست تمام سوابق امانت");
                return StatusCode(500, "خطای داخلی سرور در دریافت سوابق");
            }
        }

        // 2. دریافت یک رکورد امانت خاص بر اساس ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var borrowing = await _borrowingService.GetByIdAsync(id);
                if (borrowing == null)
                    return NotFound($"رکورد امانت با شناسه {id} یافت نشد.");

                return Ok(borrowing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت رکورد امانت با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 3. ثبت یک امانت جدید (مثلاً وقتی کتابی داده می‌شود)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BorrowingDto borrowingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _borrowingService.AddAsync(borrowingDto);

                // بازگشت وضعیت 201 و لینک به رکورد ساخته شده
                return CreatedAtAction(nameof(GetById), new { id = borrowingDto.Id }, borrowingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ثبت عملیات امانت جدید");
                return StatusCode(500, "خطای داخلی سرور در ثبت امانت");
            }
        }

        // 4. به‌روزرسانی رکورد امانت (مثلاً تغییر تاریخ بازگشت یا وضعیت کتاب)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BorrowingDto borrowingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != borrowingDto.Id)
                    return BadRequest("شناسه رکورد با اطلاعات ارسالی مطابقت ندارد.");

                await _borrowingService.UpdateAsync(borrowingDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی رکورد امانت با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 5. حذف یک رکورد امانت
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _borrowingService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف رکورد امانت با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }
    }
}

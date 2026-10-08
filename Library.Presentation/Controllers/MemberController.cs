using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        // 1. دریافت تمام اعضا
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("درخواست دریافت لیست تمام اعضا.");
                var members = await _memberService.GetAsync();
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت لیست تمام اعضا");
                return StatusCode(500, "خطای داخلی سرور در دریافت اعضا");
            }
        }

        // 2. دریافت یک عضو خاص بر اساس ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var member = await _memberService.GetByIdAsync(id);
                if (member == null)
                {
                    _logger.LogWarning("عضو با شناسه {MemberId} یافت نشد.", id);
                    return NotFound($"عضو با شناسه {id} یافت نشد.");
                }

                _logger.LogInformation("عضو با شناسه {MemberId} یافت شد.", id);
                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت عضو با شناسه {MemberId}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 3. ایجاد عضو جدید
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MemberDto memberDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("داده‌های ارسالی برای ایجاد عضو جدید نامعتبر است.");
                    return BadRequest(ModelState);
                }

                await _memberService.AddAsync(memberDto);

                // استفاده از @ برای لاگ کردن کل شیء به صورت ساختاریافته (Structured Logging)
                _logger.LogInformation("عضو جدید با موفقیت ایجاد شد: {@MemberDto}", memberDto);

                return CreatedAtAction(nameof(GetById), new { id = memberDto.Id }, memberDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد عضو جدید برای: {Name}", memberDto.Name);
                return StatusCode(500, "خطای داخلی سرور در ثبت عضو");
            }
        }

        // 4. به‌روزرسانی عضو موجود
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MemberDto memberDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != memberDto.Id)
            {
                _logger.LogWarning("تلاش برای به‌روزرسانی با IDهای متفاوت: {Id} vs {DtoId}", id, memberDto.Id);
                return BadRequest("شناسه عضو با اطلاعات ارسالی مطابقت ندارد.");
            }

            try
            {
                await _memberService.UpdateAsync(memberDto);
                _logger.LogInformation("عضو با شناسه {MemberId} با موفقیت به‌روزرسانی شد.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی عضو با شناسه {MemberId}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 5. حذف عضو
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _memberService.DeleteAsync(id);
                _logger.LogInformation("عضو با شناسه {MemberId} با موفقیت حذف شد.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف عضو با شناسه {MemberId}", id);
                return StatusCode(500, "خطای داخلی سرور در عملیات حذف");
            }
        }
    }
}


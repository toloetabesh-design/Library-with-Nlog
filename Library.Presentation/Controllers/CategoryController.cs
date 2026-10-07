using Library.Application.DTOs;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAsync();
                _logger.LogInformation("");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, "خطای داخلی سرور در دریافت دسته‌بندی‌ها");
            }
        }

        // 2. دریافت یک دسته‌بندی خاص بر اساس ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("تلاش برای دریافت دسته‌بندی با شناسه {Id} که یافت نشد.", id);
                    return NotFound($"دسته‌بندی با شناسه {id} یافت نشد.");
                }

                _logger.LogInformation("دسته‌بندی با شناسه {Id} یافت شد.", id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت دسته‌بندی با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 3. ایجاد دسته‌بندی جدید
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("تلاش برای ایجاد دسته‌بندی نامعتبر با داده‌های: {@Dto}", categoryDto);
                    return BadRequest(ModelState);
                }

                await _categoryService.AddAsync(categoryDto);

                // ثبت اطلاع از موفقیت عملیات
                _logger.LogInformation("دسته‌بندی جدید با نام '{CategoryName}' با موفقیت ایجاد شد.", categoryDto.Name);

                return CreatedAtAction(nameof(GetById), new { id = categoryDto.Id }, categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد دسته‌بندی جدید: {Name}", categoryDto.Name);
                return StatusCode(500, "خطای داخلی سرور در ثبت دسته‌بندی");
            }
        }

        // 4. به‌روزرسانی دسته‌بندی موجود
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != categoryDto.Id)
                {
                    _logger.LogWarning("تلاش برای به‌روزرسانی با IDهای متفاوت: {Id} vs {DtoId}", id, categoryDto.Id);
                    return BadRequest("شناسه دسته‌بندی با اطلاعات ارسالی مطابقت ندارد.");
                }

                await _categoryService.UpdateAsync(categoryDto);

                _logger.LogInformation("دسته‌بندی با شناسه {Id} با موفقیت به‌روزرسانی شد.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی دسته‌بندی با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        // 5. حذف دسته‌بندی
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);

                _logger.LogInformation("دسته‌بندی با شناسه {Id} با موفقیت حذف شد.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف دسته‌بندی با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور در عملیات حذف");
            }
        }
    }
}

using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task AddAsync(CategoryDto categoryDto)
        {
            // تبدیل DTO به Entity برای ذخیره در دیتابیس
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateAsync(CategoryDto categoryDto)
        {
            // ابتدا چک می‌کنیم آیا این دسته‌بندی وجود دارد یا خیر
            var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);

            if (existingCategory == null)
                throw new Exception($"Category with ID {categoryDto.Id} not found.");

            // تبدیل مقادیر DTO به Entity موجود
            _mapper.Map(categoryDto, existingCategory);

            // ذخیره تغییرات
            await _categoryRepository.UpdateAsync(existingCategory);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(id);
            }
        }
    }
}

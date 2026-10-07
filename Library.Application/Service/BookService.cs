using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        // دریافت همه کتاب‌ها
        public async Task<IEnumerable<BookDto>> GetAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            // تبدیل لیست Entity به لیست DTO
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        // دریافت یک کتاب بر اساس ID
        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return null;

            return _mapper.Map<BookDto>(book);
        }

        // اضافه کردن کتاب جدید
        public async Task AddAsync(BookDto bookDto)
        {
            
            var book = _mapper.Map<Book>(bookDto);

            await _bookRepository.AddAsync(book);
        }

        // به‌روزرسانی کتاب
        public async Task UpdateAsync(BookDto bookDto)
        {
            var existingBook = await _bookRepository.GetByIdAsync(bookDto.Id);
            if (existingBook == null)
                throw new Exception("کتاب یافت نشد.");

            // انتقال مقادیر از DTO به Entity موجود
            _mapper.Map(bookDto, existingBook);

            await _bookRepository.UpdateAsync(existingBook);
        }

        // حذف کتاب
        public async Task DeleteAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new Exception("کتاب برای حذف یافت نشد.");

            await _bookRepository.DeleteAsync(id);
        }
    }
}

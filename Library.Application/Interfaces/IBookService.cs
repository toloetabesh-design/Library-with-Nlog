using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Application.DTOs;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        // نام متد اینجا باید دقیقاً با کنترلر یکی باشد
        Task<IEnumerable<BookDto>> GetAsync();

        Task<BookDto?> GetByIdAsync(int id);
        Task AddAsync(BookDto bookDto);
        Task UpdateAsync(BookDto bookDto);
        Task DeleteAsync(int id);
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Entities;

namespace Library.Persistence.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();

        Task<Book> GetByIdAsync(int id);

        Task AddAsync(Book book);

        Task UpdateAsync(Book book);

        Task DeleteAsync(int id);
    }
}
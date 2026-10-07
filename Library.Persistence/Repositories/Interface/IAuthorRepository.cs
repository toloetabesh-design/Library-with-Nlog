using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Entities;


namespace Library.Application.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAsync();

        Task<Author?> GetByIdAsync(int id);

        Task AddAsync(Author author);

        Task UpdateAsync(Author author);

        Task DeleteAsync(int id);
    }
}
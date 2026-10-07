using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.DTOs;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAsync();

        Task<AuthorDto?> GetByIdAsync(int id);

        Task AddAsync(AuthorDto authorDto);

        Task UpdateAsync(AuthorDto authorDto);

        Task DeleteAsync(int id);
    }
}

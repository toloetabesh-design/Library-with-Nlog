using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Entities;


namespace Library.Application.Interfaces
{
    public interface IBorrowingRepository
    {
        Task<IEnumerable<Borrowing>> GetAsync();

        Task<Borrowing?> GetByIdAsync(int id);

        Task AddAsync(Borrowing borrowing);

        Task UpdateAsync(Borrowing borrowing);

        Task DeleteAsync(int id);
    }
}
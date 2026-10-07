using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.DTOs;

namespace Library.Application.Interfaces
{
    public interface IBorrowingService
    {
        Task<IEnumerable<BorrowingDto>> GetAsync();

        Task<BorrowingDto?> GetByIdAsync(int id);

        Task AddAsync(BorrowingDto borrowingDto);

        Task UpdateAsync(BorrowingDto borrowingDto);

        Task DeleteAsync(int id);

        Task<BorrowingDto> ProcessBorrowingAsync(CreateBorrowingDto request);
    }
}
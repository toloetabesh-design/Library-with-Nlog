using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly AppDbContext _context;

        public BorrowingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Borrowing>> GetAsync()
        {
            return await _context.Borrowings
                .Include(x => x.Book)
                .Include(x => x.Member)
                .ToListAsync();
        }

        public async Task<Borrowing?> GetByIdAsync(int id)
        {
            return await _context.Borrowings
                .Include(x => x.Book)
                .Include(x => x.Member)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Borrowing borrowing)
        {
            await _context.Borrowings.AddAsync(borrowing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Borrowing borrowing)
        {
            _context.Borrowings.Update(borrowing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var borrowing = await GetByIdAsync(id);

            if (borrowing != null)
            {
                _context.Borrowings.Remove(borrowing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
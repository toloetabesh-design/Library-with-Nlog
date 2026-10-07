using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IMapper _mapper;

        public BorrowingService(
            IBorrowingRepository borrowingRepository,
            IMapper mapper)
        {
            _borrowingRepository = borrowingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BorrowingDto>> GetAsync()
        {
            var borrowings = await _borrowingRepository.GetAsync();

            return _mapper.Map<IEnumerable<BorrowingDto>>(borrowings);
        }

        public async Task<BorrowingDto?> GetByIdAsync(int id)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(id);

            if (borrowing == null)
                return null;

            return _mapper.Map<BorrowingDto>(borrowing);
        }

        public async Task AddAsync(BorrowingDto borrowingDto)
        {
            var borrowing = _mapper.Map<Borrowing>(borrowingDto);

            await _borrowingRepository.AddAsync(borrowing);
        }

        public async Task UpdateAsync(BorrowingDto borrowingDto)
        {
            var borrowing = _mapper.Map<Borrowing>(borrowingDto);

            await _borrowingRepository.UpdateAsync(borrowing);
        }

        public async Task DeleteAsync(int id)
        {
            await _borrowingRepository.DeleteAsync(id);
        }

        public async Task<BorrowingDto> ProcessBorrowingAsync(CreateBorrowingDto request)
        {
            var borrowing = new Borrowing
            {
                BookId = request.BookId,
                MemberId = request.MemberId,
                BorrowDate = DateTime.UtcNow
            };

            await _borrowingRepository.AddAsync(borrowing);

            return _mapper.Map<BorrowingDto>(borrowing);
        }
    }
}
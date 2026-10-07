using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorDto>> GetAsync()
        {
            var authors = await _authorRepository.GetAsync();

            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task<AuthorDto?> GetByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
                return null;

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task AddAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            await _authorRepository.AddAsync(author);

        }

        public async Task UpdateAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            await _authorRepository.UpdateAsync(author);
        }

        public async Task DeleteAsync(int id)
        {
            await _authorRepository.DeleteAsync(id);
        }
    }
}

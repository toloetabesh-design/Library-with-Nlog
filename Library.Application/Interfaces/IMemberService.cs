using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.DTOs;

namespace Library.Application.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAsync();

        Task<MemberDto?> GetByIdAsync(int id);

        Task AddAsync(MemberDto memberDto);

        Task UpdateAsync(MemberDto memberDto);

        Task DeleteAsync(int id);
    }
}
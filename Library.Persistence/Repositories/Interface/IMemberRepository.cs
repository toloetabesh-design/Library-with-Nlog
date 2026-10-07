using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Entities;



namespace Library.Application.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAsync();

        Task<Member?> GetByIdAsync(int id);

        Task AddAsync(Member member);

        Task UpdateAsync(Member member);

        Task DeleteAsync(int id);
    }
}
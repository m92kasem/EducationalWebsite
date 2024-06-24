using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;

namespace EducationalWebsite.Domain.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz> GetByIdAsync(Guid Id);
        Task<IEnumerable<Quiz>> GetAllAsync();
        Task AddAsync(Quiz quiz);
        Task UpdateAsync(Guid Id, Quiz quiz);
        Task DeleteAsync(Guid id);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities.Questions;

namespace EducationalWebsite.Domain.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> GetByIdAsync(Guid Id);
        Task<IEnumerable<Question>> GetAllAsync();
        Task AddAsync(Question question);
        Task UpdateAsync(Guid Id, Question question);
        Task DeleteAsync(Guid id);
    }
}
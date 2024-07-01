using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;

namespace EducationalWebsite.Domain.Interfaces
{
    public interface ITestRepository
    {
        Task<TestQ> GetByIdAsync(Guid Id);
        Task<IEnumerable<TestQ>> GetAllAsync();
        Task AddAsync(TestQ test);
        Task UpdateAsync(Guid Id, TestQ test);
        Task DeleteAsync(TestQ id);
    }

}
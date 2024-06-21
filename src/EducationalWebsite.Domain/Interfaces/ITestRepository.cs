using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalWebsite.Domain.Interfaces
{
    public interface ITestRepository
    {
        Task<Test> GetByIdAsync(Guid id);
        Task<IEnumerable<Test>> GetAllAsync();
        Task AddAsync(Test test);
        Task UpdateAsync(Test test);
        Task DeleteAsync(Guid id);
    }

    public class Test
    {
    }
}
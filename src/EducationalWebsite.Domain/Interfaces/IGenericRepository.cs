using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalWebsite.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(Guid id, T entity);
        Task UpdatePartialAsync(Guid id, Dictionary<string, object> updates);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
        
    }
}
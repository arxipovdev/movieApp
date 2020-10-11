using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Repositories
{
    public interface IProducerRepository
    {
        Task<IEnumerable<Producer>> GetAllAsync();
        Task<Producer> GetByIdAsync(int id);
        Task<bool> CreateAsync(Producer producer);
        Task<bool> UpdateAsync(Producer producer);
        Task<bool> DeleteAsync(int id);
        bool CheckUser(Producer producer);
    }
}
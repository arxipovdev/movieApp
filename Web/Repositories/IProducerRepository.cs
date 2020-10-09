using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Repositories
{
    public interface IProducerRepository : IRepository<Producer>
    {
        // Task<IEnumerable<Producer>> GetAll();
        // Task<Producer> GetById(int id);
        // Task<bool> Create(Producer producer);
        // Task<bool> Update(Producer producer);
        // Task<bool> Delete(int id);
    }
}
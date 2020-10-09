using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        public Task<IEnumerable<Producer>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Producer> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Create(Producer producer)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(Producer producer)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
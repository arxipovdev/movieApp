using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        public Task<IEnumerable<Movie>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Movie> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Create(Movie entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(Movie entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
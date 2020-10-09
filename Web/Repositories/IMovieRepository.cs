using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        // Task<IEnumerable<Movie>> GetAll();
        // Task<Movie> GetById(int id);
        // Task<bool> Create(Movie producer);
        // Task<bool> Update(Movie producer);
        // Task<bool> Delete(int id);
    }
}
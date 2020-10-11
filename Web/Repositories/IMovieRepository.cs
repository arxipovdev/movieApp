using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;
using Web.ViewModels;

namespace Web.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(int id);
        Task<bool> CreateAsync(Movie movie);
        Task<bool> UpdateAsync(Movie movie);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Movie>> GetPaginateAsync(int numberPage, int sizePage);
        Task<int> CountAsync();
        bool CheckUser(Movie movie);
    }
}
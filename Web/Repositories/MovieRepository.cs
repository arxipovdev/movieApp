using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _db.Movies
                .Include(x => x.Producer)
                .Where(x => x.DeleteAt == null)
                .OrderByDescending(x => x.CreateAt)
                .ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _db.Movies.FindAsync(id);
        }

        public async Task<bool> Create(Movie movie)
        {
            movie.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            movie.CreateAt = movie.UpdateAt = DateTime.Now;
            await _db.AddAsync(movie);
            var created = await _db.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> Update(Movie movie)
        {
            movie.UpdateAt = DateTime.Now;
            _db.Update(movie);
            var updated = await _db.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var movie = await GetById(id);
            movie.DeleteAt = DateTime.Now;
            _db.Update(movie);
            var deleted = await _db.SaveChangesAsync();
            return deleted > 0;
        }

        public bool CheckUser(Movie movie)
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value == movie.UserId;
        }
    }
}
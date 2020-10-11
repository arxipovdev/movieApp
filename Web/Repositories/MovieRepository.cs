using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using Web.ViewModels;

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
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _db.Movies
                .Include(x => x.Producer)
                .Where(x => x.DeleteAt == null)
                .OrderByDescending(x => x.CreateAt)
                .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _db.Movies.FindAsync(id);
        }

        public async Task<bool> CreateAsync(Movie movie)
        {
            movie.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            movie.CreateAt = movie.UpdateAt = DateTime.Now;
            await _db.AddAsync(movie);
            var created = await _db.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdateAsync(Movie movie)
        {
            movie.UpdateAt = DateTime.Now;
            _db.Update(movie);
            var updated = await _db.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await GetByIdAsync(id);
            movie.DeleteAt = DateTime.Now;
            _db.Update(movie);
            var deleted = await _db.SaveChangesAsync();
            return deleted > 0;
        }

        public bool CheckUser(Movie movie)
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value == movie.UserId;
        }

        public async Task<int> CountAsync()
        {
            return await _db.Movies.CountAsync();
        }
        
        public async Task<IEnumerable<Movie>> GetPaginateAsync(int numberPage, int sizePage)
        {
            return await _db.Movies
                .Where(x => x.DeleteAt == null)
                .Include(x => x.Producer)
                .OrderByDescending(x => x.CreateAt)
                .Skip((numberPage - 1) * sizePage)
                .Take(sizePage)
                .ToListAsync();
        }
    }
}
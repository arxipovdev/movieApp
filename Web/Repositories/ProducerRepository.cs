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
    public class ProducerRepository : IProducerRepository
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProducerRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<Producer>> GetAllAsync()
        {
            return await _db.Producers
                .Where(x => x.DeleteAt == null)
                .OrderByDescending(x => x.CreateAt)
                .ToListAsync();
        }

        public async Task<Producer> GetByIdAsync(int id)
        {
            return await _db.Producers.FindAsync(id);
        }

        public async Task<bool> CreateAsync(Producer producer)
        {
            producer.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            producer.CreateAt = producer.UpdateAt = DateTime.Now;
            await _db.AddAsync(producer);
            var created = await _db.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdateAsync(Producer producer)
        {
            producer.UpdateAt = DateTime.Now;
            _db.Update(producer);
            var updated = await _db.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producer = await GetByIdAsync(id);
            producer.DeleteAt = DateTime.Now;
            _db.Update(producer);
            var deleted = await _db.SaveChangesAsync();
            return deleted > 0;
        }

        public bool CheckUser(Producer producer)
        {
            return _httpContextAccessor
                .HttpContext
                .User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value == producer.UserId;
        }
    }
}
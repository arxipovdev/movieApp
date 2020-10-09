using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private readonly string _userId;

        public ProducerRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public async Task<IEnumerable<Producer>> GetAll()
        {
            return await _db.Producers
                .Where(x => x.DeleteAt == null)
                .OrderByDescending(x => x.CreateAt)
                .ToListAsync();
        }

        public async Task<Producer> GetById(int id)
        {
            return await _db.Producers.FindAsync(id);
        }

        public async Task<bool> Create(Producer producer)
        {
            producer.UserId = _userId;
            producer.CreateAt = producer.UpdateAt = DateTime.Now;
            await _db.AddAsync(producer);
            var created = await _db.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> Update(Producer producer)
        {
            producer.UpdateAt = DateTime.Now;
            _db.Update(producer);
            var updated = await _db.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var producer = await GetById(id);
            producer.DeleteAt = DateTime.Now;
            _db.Update(producer);
            var deleted = await _db.SaveChangesAsync();
            return deleted > 0;
        }

        public bool CheckUser(Producer producer)
        {
            return _userId == producer.UserId;
        }
    }
}
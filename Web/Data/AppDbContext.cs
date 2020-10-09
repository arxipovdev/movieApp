using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Movie> Movies { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    }
}
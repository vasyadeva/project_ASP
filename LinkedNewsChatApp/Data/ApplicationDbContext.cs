using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkedNewsChatApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<News> news => Set<News>();
    }

}
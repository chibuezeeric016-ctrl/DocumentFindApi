using Microsoft.EntityFrameworkCore;
using DocumentFindApi.Models;

namespace DocumentFindApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Document> Documents { get; set; }
    
    }
}

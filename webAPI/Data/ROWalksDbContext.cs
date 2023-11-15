using Microsoft.EntityFrameworkCore;
using webAPI.Models.Domain;

namespace webAPI.Data
{
    public class ROWalksDbContext: DbContext
    {
        public ROWalksDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

    }
}

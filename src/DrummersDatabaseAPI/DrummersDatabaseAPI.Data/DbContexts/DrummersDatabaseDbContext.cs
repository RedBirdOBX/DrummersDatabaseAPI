using DrummersDatabaseAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrummersDatabaseAPI.Data.DbContexts
{
    public class DrummersDatabaseDbContext : DbContext
    {
        // constructor
        public DrummersDatabaseDbContext(DbContextOptions<DrummersDatabaseDbContext> options) : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Entry> Entries { get; set; }
    }
}

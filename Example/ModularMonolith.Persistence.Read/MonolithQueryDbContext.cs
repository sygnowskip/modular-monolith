using System.Linq;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Persistence.Read.Configurations;
using ModularMonolith.ReadModels;

namespace ModularMonolith.Persistence.Read
{
    public class MonolithQueryDbContext : DbContext, IMonolithQueryDbContext
    {
        public MonolithQueryDbContext(DbContextOptions<MonolithQueryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }
        
        public IQueryable<Location> Locations => Set<Location>().AsQueryable();
        public IQueryable<Subject> Subjects => Set<Subject>().AsQueryable();
    }
}
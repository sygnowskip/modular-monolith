using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.EntityFrameworkCore.Inbox.Entities;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Exams.Persistence.Configurations;
using ModularMonolith.Persistence.Configurations;
using ModularMonolith.ReadModels.Persistence.Common;

namespace ModularMonolith.Persistence
{
    internal partial class MonolithDbContext : DbContext, ITransactionProvider
    {
        public MonolithDbContext(DbContextOptions<MonolithDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExamConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RegistrationEntityConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocationConfiguration).Assembly);
            modelBuilder.ApplyConfiguration(new RegistrationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SerializedEventEntityConfig());
            modelBuilder.ApplyConfiguration(new ProcessedEventEntityConfig());
        }

        public Task BeginTransactionAsync()
        {
            return Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await SaveChangesAsync();
            await Database.CommitTransactionAsync();
        }

        public Task RollbackTransactionAsync()
        {
            return Database.RollbackTransactionAsync();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Hexure.Deleting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Hexure.EntityFrameworkCore.Deleting
{
    public class DeleteAggregatesOnSaveChangesInterceptor : ISaveChangesInterceptor
    {
        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DeleteAggregates(eventData.Context.ChangeTracker);
            return result;
        }

        public async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            DeleteAggregates(eventData.Context.ChangeTracker);
            return result;
        }
        
        private void DeleteAggregates(ChangeTracker changeTracker)
        {
            var aggregates = changeTracker.Entries<IDeletableAggregate>();

            foreach (var aggregate in aggregates)
            {
                if (aggregate.Entity.IsDeleted)
                {
                    aggregate.State = aggregate.State == EntityState.Added ? EntityState.Detached : EntityState.Deleted;
                }
            }
        }
        
        #region default implementations

        public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            return result;
        }
        
        public void SaveChangesFailed(DbContextErrorEventData eventData)
        {
        }

        public ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return new ValueTask<int>(result);
        }

        public Task SaveChangesFailedAsync(DbContextErrorEventData eventData,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
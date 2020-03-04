using System.Linq;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Payments;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    public class MonolithQueryDbContext
    {
        private readonly MonolithDbContext _dbContext;

        internal MonolithQueryDbContext(MonolithDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Payment> Payments => _dbContext.Payments.AsNoTracking();
        public IQueryable<Registration> Registrations => _dbContext.Registrations.AsNoTracking();
    }
}
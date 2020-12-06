using System.Linq;
using Hexure.EntityFrameworkCore.Events.Entites;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Persistence;
using ModularMonolith.Payments;
using ModularMonolith.ReadModels;
using ModularMonolith.ReadModels.Common;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    internal partial class MonolithDbContext : IExamDbContext, IMonolithQueryDbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<SerializedEventEntity> SerializedEvents { get; set; }
        public DbSet<Exam> Exams { get; set; }
        internal DbSet<Location> Locations { get; set; }
        internal DbSet<Subject> Subjects { get; set; }

        IQueryable<ReadModels.Planning.Exam> IMonolithQueryDbContext.Exams => Set<ReadModels.Planning.Exam>().AsNoTracking().AsQueryable();
        IQueryable<Location> IMonolithQueryDbContext.Locations => Locations.AsNoTracking().AsQueryable();
        IQueryable<Subject> IMonolithQueryDbContext.Subjects => Subjects.AsNoTracking().AsQueryable();
    }
}
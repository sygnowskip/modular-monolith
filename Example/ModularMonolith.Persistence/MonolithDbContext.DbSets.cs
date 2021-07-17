using System.Linq;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.EntityFrameworkCore.Inbox;
using Hexure.EntityFrameworkCore.Inbox.Entities;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Persistence;
using ModularMonolith.Payments;
using ModularMonolith.ReadModels;
using ModularMonolith.ReadModels.Common;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence
{
    internal partial class MonolithDbContext : IExamDbContext, IMonolithQueryDbContext,  ISerializedEventDbContext, IInboxDbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<SerializedEventEntity> SerializedEvents { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ProcessedEventEntity> ProcessedEvents { get; set; }
        internal DbSet<Location> Locations { get; set; }
        internal DbSet<Subject> Subjects { get; set; }

        IQueryable<Location> IMonolithQueryDbContext.Locations => Locations.AsNoTracking().AsQueryable();
        IQueryable<Subject> IMonolithQueryDbContext.Subjects => Subjects.AsNoTracking().AsQueryable();
    }
}
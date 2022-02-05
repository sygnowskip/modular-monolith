using System.Linq;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.EntityFrameworkCore.Inbox;
using Hexure.EntityFrameworkCore.Inbox.Entities;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Persistence;
using ModularMonolith.Orders.Domain;
using ModularMonolith.Orders.Persistence;
using ModularMonolith.ReadModels;
using ModularMonolith.ReadModels.Common;
using ModularMonolith.Registrations.Domain;
using ModularMonolith.Registrations.Persistence;

namespace ModularMonolith.Persistence
{
    internal partial class MonolithDbContext : IExamsDbContext, IMonolithQueryDbContext, ISerializedEventDbContext,
        IInboxDbContext, IRegistrationsDbContext, IOrdersDbContext
    {
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProcessedEventEntity> ProcessedEvents { get; set; }
        public DbSet<SerializedEventEntity> SerializedEvents { get; set; }
        internal DbSet<Location> Locations { get; set; }
        internal DbSet<Subject> Subjects { get; set; }

        IQueryable<Location> IMonolithQueryDbContext.Locations => Locations.AsNoTracking().AsQueryable();
        IQueryable<Subject> IMonolithQueryDbContext.Subjects => Subjects.AsNoTracking().AsQueryable();
    }
}
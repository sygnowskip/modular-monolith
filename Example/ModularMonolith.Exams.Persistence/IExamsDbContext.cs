﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Exams.Domain;

namespace ModularMonolith.Exams.Persistence
{
    public interface IExamsDbContext
    {
        DbSet<Exam> Exams { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
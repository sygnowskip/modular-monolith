using System;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore.Inbox.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hexure.EntityFrameworkCore.Inbox.Repositories
{
    public interface IProcessedEventRepository
    {
        Task<bool> ExistsAsync(Guid messageId, string consumer);
        Task AddAsync(ProcessedEventEntity processedEvent);
    }

    public class ProcessedEventRepository : IProcessedEventRepository
    {
        private readonly IInboxDbContext _inboxDbContext;

        public ProcessedEventRepository(IInboxDbContext inboxDbContext)
        {
            _inboxDbContext = inboxDbContext;
        }

        public Task<bool> ExistsAsync(Guid messageId, string consumer)
        {
            return _inboxDbContext.ProcessedEvents.AnyAsync(pe => pe.MessageId == messageId && pe.Consumer == consumer);
        }

        public async Task AddAsync(ProcessedEventEntity processedEvent)
        {
            await _inboxDbContext.ProcessedEvents.AddAsync(processedEvent);
            await _inboxDbContext.SaveChangesAsync();
        }
    } 
}
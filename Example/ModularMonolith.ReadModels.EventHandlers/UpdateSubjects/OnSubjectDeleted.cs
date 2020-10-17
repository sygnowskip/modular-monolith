using System.Threading.Tasks;
using ExternalSystem.Events.Subjects;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Language.Subjects;
using ModularMonolith.Persistence;

namespace ModularMonolith.ReadModels.EventHandlers.UpdateSubjects
{
    internal class OnSubjectDeleted : IConsumer<SubjectDeleted>
    {
        private readonly MonolithDbContext _monolithDbContext;

        public OnSubjectDeleted(MonolithDbContext monolithDbContext)
        {
            _monolithDbContext = monolithDbContext;
        }

        public async Task Consume(ConsumeContext<SubjectDeleted> context)
        {
            var subject = await _monolithDbContext.Subjects.SingleOrDefaultAsync(l => l.Id == new SubjectId(context.Message.Id));
            if (subject == null)
                return;

            _monolithDbContext.Subjects.Remove(subject);
            await _monolithDbContext.SaveChangesAsync();
        }
    }
}
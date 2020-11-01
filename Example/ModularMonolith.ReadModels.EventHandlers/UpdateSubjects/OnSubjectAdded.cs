using System.Threading.Tasks;
using ExternalSystem.Events.Subjects;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Language.Subjects;
using ModularMonolith.Persistence;

namespace ModularMonolith.ReadModels.EventHandlers.UpdateSubjects
{
    internal class OnSubjectAdded : IConsumer<SubjectAdded>
    {
        private readonly MonolithDbContext _monolithDbContext;

        public OnSubjectAdded(MonolithDbContext monolithDbContext)
        {
            _monolithDbContext = monolithDbContext;
        }

        public async Task Consume(ConsumeContext<SubjectAdded> context)
        {
            var subjectExist = await _monolithDbContext.Subjects.AnyAsync(l => l.Id == new SubjectId(context.Message.Id));
            if (subjectExist)
                return;

            await _monolithDbContext.Subjects.AddAsync(new Subject(new SubjectId(context.Message.Id), 
                context.Message.Name));
            await _monolithDbContext.SaveChangesAsync();
        }
    }
}
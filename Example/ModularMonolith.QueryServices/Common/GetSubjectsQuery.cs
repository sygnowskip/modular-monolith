using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ModularMonolith.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace ModularMonolith.QueryServices
{
    public class SubjectDto
    {
        public SubjectDto(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; }
        public string Name { get; }
    }
    
    public class GetSubjectsQuery : IRequest<IReadOnlyCollection<SubjectDto>>
    {
        
    }

    public class GetSubjectsQueryHandler : IRequestHandler<GetSubjectsQuery, IReadOnlyCollection<SubjectDto>>
    {
        private readonly IMonolithQueryDbContext _monolithQueryDbContext;

        public GetSubjectsQueryHandler(IMonolithQueryDbContext monolithQueryDbContext)
        {
            _monolithQueryDbContext = monolithQueryDbContext;
        }

        public async Task<IReadOnlyCollection<SubjectDto>> Handle(GetSubjectsQuery request, CancellationToken cancellationToken)
        {
            return await _monolithQueryDbContext.Subjects
                .Select(subject => new SubjectDto(subject.Id.Value, subject.Name))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
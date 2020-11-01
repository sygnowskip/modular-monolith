using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.ReadModels;

namespace ModularMonolith.QueryServices
{
    public class LocationDto
    {
        public LocationDto(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; }
        public string Name { get; }
    }
    
    public class GetLocationsQuery : IRequest<IReadOnlyCollection<LocationDto>>
    {
        
    }

    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, IReadOnlyCollection<LocationDto>>
    {
        private readonly IMonolithQueryDbContext _monolithQueryDbContext;

        public GetLocationsQueryHandler(IMonolithQueryDbContext monolithQueryDbContext)
        {
            _monolithQueryDbContext = monolithQueryDbContext;
        }

        public async Task<IReadOnlyCollection<LocationDto>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            return await _monolithQueryDbContext.Locations
                .Select(location => new LocationDto(location.Id.Value, location.Name))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
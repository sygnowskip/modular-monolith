using System.Linq;
using ModularMonolith.ReadModels.Common;

namespace ModularMonolith.ReadModels
{
    public interface IMonolithQueryDbContext
    {
        IQueryable<Location> Locations { get; }
        IQueryable<Subject> Subjects { get; }
    }
}
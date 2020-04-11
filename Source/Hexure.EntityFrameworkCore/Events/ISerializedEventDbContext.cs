using Hexure.EntityFrameworkCore.Events.Entites;
using Microsoft.EntityFrameworkCore;

namespace Hexure.EntityFrameworkCore.Events
{
    public interface ISerializedEventDbContext
    {
        DbSet<SerializedEventEntity> SerializedEvents { get; }
    }
}
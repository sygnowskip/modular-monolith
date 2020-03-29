using Hexure.EntityFrameworkCore.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hexure.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder EnableIdentifiers(this DbContextOptionsBuilder builder)
        {
            return builder
                .ReplaceService<IValueConverterSelector, IdentifiersValueConverterSelector>();
        }
    }
}
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;

namespace Hexure.EntityFrameworkCore.SqlServer.Extensions
{
    public static class PropertyBuilderExtensions
    {
        public static void IdentifierGeneratedOnAdd<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
        {
            propertyBuilder
                .HasAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy, SqlServerValueGenerationStrategy.None)
                .ValueGeneratedOnAdd();
        }
    }
}
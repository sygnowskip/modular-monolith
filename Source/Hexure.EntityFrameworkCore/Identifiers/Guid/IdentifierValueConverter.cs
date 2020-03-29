using Hexure.Identifiers.Common;
using Hexure.Identifiers.Guid;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hexure.EntityFrameworkCore.Identifiers.Guid
{
    internal class IdentifierValueConverter<TIdentifier> : ValueConverter<TIdentifier, System.Guid>
        where TIdentifier : Identifier
    {
        public IdentifierValueConverter()
            : base(id => id.Value, value => Create(value))
        {
        }

        private static TIdentifier Create(System.Guid id) => IdentifierActivator.Create(typeof(TIdentifier), id) as TIdentifier;
    }
}
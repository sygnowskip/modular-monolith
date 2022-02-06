using Hexure.EntityFrameworkCore.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Orders.Domain;
using ModularMonolith.Orders.Domain.ValueObjects;
using ModularMonolith.Orders.Language;

namespace ModularMonolith.Orders.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order), Schemas.Orders);
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .IdentifierGeneratedOnAdd();
            builder.Property(o => o.DomainTimestamp)
                .IsConcurrencyToken();
            
            builder.OwnsOne(o => o.CreationDateTime, navigationBuilder =>
            {
                navigationBuilder.Property(cd => cd.Value)
                    .HasColumnName(nameof(Order.CreationDateTime));
            });
            builder.Navigation(o => o.CreationDateTime).IsRequired();
            
            builder.OwnsOne(o => o.Seller, navigationBuilder =>
            {
                navigationBuilder.Property(s => s.City)
                    .HasColumnName($"{nameof(Order.Seller)}{nameof(ContactData.City)}");
                navigationBuilder.Property(s => s.Name)
                    .HasColumnName($"{nameof(Order.Seller)}{nameof(ContactData.Name)}");
                navigationBuilder.Property(s => s.StreetAddress)
                    .HasColumnName($"{nameof(Order.Seller)}{nameof(ContactData.StreetAddress)}");
                navigationBuilder.Property(s => s.ZipCode)
                    .HasColumnName($"{nameof(Order.Seller)}{nameof(ContactData.ZipCode)}");
            });
            builder.Navigation(o => o.Seller).IsRequired();
            
            builder.OwnsOne(o => o.Buyer, navigationBuilder =>
            {
                navigationBuilder.Property(s => s.City)
                    .HasColumnName($"{nameof(Order.Buyer)}{nameof(ContactData.City)}");
                navigationBuilder.Property(s => s.Name)
                    .HasColumnName($"{nameof(Order.Buyer)}{nameof(ContactData.Name)}");
                navigationBuilder.Property(s => s.StreetAddress)
                    .HasColumnName($"{nameof(Order.Buyer)}{nameof(ContactData.StreetAddress)}");
                navigationBuilder.Property(s => s.ZipCode)
                    .HasColumnName($"{nameof(Order.Buyer)}{nameof(ContactData.ZipCode)}");
            });
            builder.Navigation(o => o.Buyer).IsRequired();
            
            builder.OwnsOne(o => o.Summary, navigationBuilder =>
            {
                navigationBuilder.OwnsOne(s => s.Net, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.Property(p => p.Amount)
                        .HasColumnName($"{nameof(Price.Net)}{nameof(Money.Amount)}")
                        .HasPrecision(7, 3);
                    ownedNavigationBuilder.OwnsOne(p => p.Currency, currencyBuilder =>
                    {
                        currencyBuilder.Property(p => p.ShortCode)
                            .HasColumnName($"{nameof(Price.Net)}{nameof(Money.Currency)}");
                    });
                    ownedNavigationBuilder.Navigation(p => p.Currency).IsRequired();
                });
                navigationBuilder.Navigation(p => p.Net).IsRequired();
                navigationBuilder.OwnsOne(s => s.Tax, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.Property(p => p.Amount)
                        .HasColumnName($"{nameof(Price.Tax)}{nameof(Money.Amount)}")
                        .HasPrecision(7, 3);
                    ownedNavigationBuilder.OwnsOne(p => p.Currency, currencyBuilder =>
                    {
                        currencyBuilder.Property(p => p.ShortCode)
                            .HasColumnName($"{nameof(Price.Tax)}{nameof(Money.Currency)}");
                    });
                    ownedNavigationBuilder.Navigation(p => p.Currency).IsRequired();
                });
                navigationBuilder.Navigation(p => p.Tax).IsRequired();
                navigationBuilder.Ignore(s => s.Gross);
            });
            builder.Navigation(o => o.Summary).IsRequired();
            
            builder.OwnsMany(o => o.Items, navigationBuilder =>
            {
                navigationBuilder.WithOwner().HasForeignKey(nameof(OrderId));
                navigationBuilder.ToTable(nameof(OrderItem), Schemas.Orders);
                navigationBuilder.HasVirtualPrimaryKey();

                navigationBuilder.Property(i => i.Name);
                navigationBuilder.Property(i => i.ExternalId);
                navigationBuilder.Property(i => i.ProductType);
                navigationBuilder.OwnsOne(i => i.Quantity, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.Property(q => q.Value)
                        .HasColumnName(nameof(Quantity));
                });
                navigationBuilder.Navigation(o => o.Quantity).IsRequired();
                
                navigationBuilder.OwnsOne(o => o.Price, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.OwnsOne(s => s.Net, ownedOwnedNavigationBuilder =>
                    {
                        ownedOwnedNavigationBuilder.Property(p => p.Amount)
                            .HasColumnName($"{nameof(Price.Net)}{nameof(Money.Amount)}")
                            .HasPrecision(7, 3);
                        ownedOwnedNavigationBuilder.OwnsOne(p => p.Currency, currencyBuilder =>
                        {
                            currencyBuilder.Property(p => p.ShortCode)
                                .HasColumnName($"{nameof(Price.Net)}{nameof(Money.Currency)}");
                        });
                        ownedOwnedNavigationBuilder.Navigation(p => p.Currency).IsRequired();
                    });
                    ownedNavigationBuilder.Navigation(p => p.Net).IsRequired();
                    ownedNavigationBuilder.OwnsOne(s => s.Tax, ownedOwnedNavigationBuilder =>
                    {
                        ownedOwnedNavigationBuilder.Property(p => p.Amount)
                            .HasColumnName($"{nameof(Price.Tax)}{nameof(Money.Amount)}")
                            .HasPrecision(7, 3);
                        ownedOwnedNavigationBuilder.OwnsOne(p => p.Currency, currencyBuilder =>
                        {
                            currencyBuilder.Property(p => p.ShortCode)
                                .HasColumnName($"{nameof(Price.Tax)}{nameof(Money.Currency)}");
                        });
                        ownedOwnedNavigationBuilder.Navigation(p => p.Currency).IsRequired();
                    });
                    ownedNavigationBuilder.Navigation(p => p.Tax).IsRequired();
                    ownedNavigationBuilder.Ignore(s => s.Gross);
                });
                navigationBuilder.Navigation(o => o.Price).IsRequired();
            });
            builder.Property(e => e.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>());
        }
    }
}
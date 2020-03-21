using System;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Payments.ApplicationServices;
using ModularMonolith.Persistence;
using ModularMonolith.Persistence.Read;
using ModularMonolith.Registrations.ApplicationServices;

namespace ModularMonolith
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddRegistrations()
                .AddPayments()
                .AddPersistence()
                .AddReadModelsPersistence();

            Console.WriteLine("Hello World!");
        }
    }
}

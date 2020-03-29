using System;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using ModularMonolith.Configuration;

namespace ModularMonolith.Database
{
    public class Program
    {
        public static int Main()
        {
            var configuration = ApplicationSettingsConfigurationProvider.Get();
            var connectionString = configuration.GetConnectionString("Database");

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}

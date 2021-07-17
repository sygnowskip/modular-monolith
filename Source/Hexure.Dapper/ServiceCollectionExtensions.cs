using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.Dapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbConnection(this IServiceCollection services, string readDatabaseConnectionString)
        {
            services.AddTransient<IDbConnection>(provider => new SqlConnection(readDatabaseConnectionString));
            return services;
        }
    }
}
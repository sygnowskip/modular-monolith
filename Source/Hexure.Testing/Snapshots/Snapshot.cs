using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Hexure.Testing.Snapshots
{
    public class Snapshot
    {
        private readonly SqlConnectionStringBuilder _connectionStringBuilder;
        private readonly ILogger<Snapshot> _logger;

        public Snapshot(string connectionString, ILogger<Snapshot> logger)
        {
            _logger = logger;
            _connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        public async Task CreateAsync()
        {
            _logger.LogInformation($"Calling {nameof(Snapshot)}.{nameof(CreateAsync)} method");
            await ExecuteCommandAsync(SnapshotSqlCommands.CreateSnapshotCommand);
        }

        public async Task RestoreAsync()
        {
            _logger.LogInformation($"Calling {nameof(Snapshot)}.{nameof(RestoreAsync)} method");
            await ExecuteCommandAsync(SnapshotSqlCommands.RestoreSnapshotCommand);
        }

        public async Task DeleteAsync()
        {
            _logger.LogInformation($"Calling {nameof(Snapshot)}.{nameof(DeleteAsync)} method");
            await ExecuteCommandAsync(SnapshotSqlCommands.DeleteSnapshotCommand);
        }

        private async Task ExecuteCommandAsync(Func<string, string> createCommandOnDatabase)
        {
            _logger.LogInformation($"With connection string = {_connectionStringBuilder}");
            _logger.LogInformation($"With master connection string = {MasterDatabaseConnectionString()}");
            using (var connection = new SqlConnection(MasterDatabaseConnectionString()))
            {
                await connection.OpenAsync();
                var sqlCommand = new SqlCommand(createCommandOnDatabase(_connectionStringBuilder.InitialCatalog), connection);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        private string MasterDatabaseConnectionString()
        {
            var connectionString = _connectionStringBuilder.ToString();
            return connectionString.Replace(_connectionStringBuilder.InitialCatalog, "master");
        }
    }
}
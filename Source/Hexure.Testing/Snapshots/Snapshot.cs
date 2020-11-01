using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Hexure.Testing.Snapshots
{
    public class Snapshot
    {
        private readonly SqlConnectionStringBuilder _connectionStringBuilder;

        public Snapshot(string connectionString)
        {
            _connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        public async Task CreateAsync()
        {
            await ExecuteCommandAsync(SnapshotSqlCommands.CreateSnapshotCommand);
        }

        public async Task RestoreAsync()
        {
            await ExecuteCommandAsync(SnapshotSqlCommands.RestoreSnapshotCommand);
        }

        public async Task DeleteAsync()
        {
            await ExecuteCommandAsync(SnapshotSqlCommands.DeleteSnapshotCommand);
        }

        private async Task ExecuteCommandAsync(Func<string, string> createCommandOnDatabase)
        {
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
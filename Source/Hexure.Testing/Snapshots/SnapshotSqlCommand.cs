namespace Hexure.Testing.Snapshots
{
    internal static class SnapshotSqlCommands
    {
        internal static string CreateSnapshotCommand(string database)
        {
            var databaseName = DatabaseName(database);
            var snapshotName = SnapshotName(database);

            return $"DECLARE @FileLocation NVARCHAR(512);\r\n" +
                $"SET @FileLocation = CONVERT(NVARCHAR(512),SERVERPROPERTY('instancedefaultdatapath')) + '{snapshotName}.ss';\r\n" +

                "DECLARE @CreateSnapshotSql NVARCHAR(500);\r\n" +
                "SELECT @CreateSnapshotSql = \r\n" +
                $"'CREATE DATABASE [{snapshotName}]\r\n" +
                "ON (\r\n" +
                $"   NAME = {databaseName},\r\n" +
                "   FILENAME =  ''' + @FileLocation + '''\r\n" +
                ")\r\n" +
                $"AS SNAPSHOT OF [{databaseName}]';\r\n" +

                "EXEC(@CreateSnapshotSql)\r\n";
        }

        internal static string DeleteSnapshotCommand(string database)
        {
            var snapshotName = SnapshotName(database);

            return "DECLARE @Sql NVARCHAR(MAX);\r\n" +
                $"SET @Sql = 'DROP DATABASE IF EXISTS {snapshotName}';\r\n" +
                "EXEC(@Sql)";
        }

        internal static string RestoreSnapshotCommand(string database)
        {
            var databaseName = DatabaseName(database);
            var snapshotName = SnapshotName(database);

            return "DECLARE @Sql NVARCHAR(MAX);\r\n" +
                $"SET @Sql = 'ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE';\r\n" +
                "EXEC(@Sql);\r\n" +

                $"RESTORE DATABASE {databaseName}\r\n" +
                $"FROM DATABASE_SNAPSHOT = '{snapshotName}';\r\n" +


                $"SET @Sql = 'ALTER DATABASE [{databaseName}] SET MULTI_USER';\r\n" +

                "EXEC(@Sql)";
        }

        private static string DatabaseName(string database)
        {
            return database.TrimStart('[').TrimEnd(']');
        }

        private static string SnapshotName(string database)
        {
            return $"{DatabaseName(database)}_SNAPSHOT";
        }
    }
}
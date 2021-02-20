using Microsoft.Data.SqlClient;

namespace Hexure.EntityFrameworkCore.SqlServer.Inbox
{
    public static class SqlExceptionExtensions
    {
        //https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/cc645728(v=sql.105)
        public static bool IsAlreadyProcessedException(this SqlException sqlException)
        {
            return sqlException.Number == 2601 && sqlException.Message.Contains("UQ_ProcessedEvent_MessageId_Consumer");
        }
    }
}
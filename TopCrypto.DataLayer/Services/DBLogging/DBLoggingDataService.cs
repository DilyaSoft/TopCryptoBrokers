using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.DBLogging.Interfaces;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.DBLogging
{
    public class DBLoggingDataService : AbstractSQLDataService, IDBLoggingDataService
    {
        public DBLoggingDataService(IConfiguration config) : base(config)
        {

        }

        public async Task WriteLog(string stackTrace, string message, string actionName, string exceptionType)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"INSERT INTO [dbo].[DbLog]
           ([StackTrace]
           ,[Message]
           ,[ActionName]
           ,[EceptionType])
     VALUES
           (@StackTrace
           ,@Message
           ,@ActionName
           ,@ExceptionType)", conn))
                {

                    var parameters = comm.Parameters;
                    parameters.AddWithValue("@StackTrace", stackTrace);
                    parameters.AddWithValue("@Message", message);
                    parameters.AddWithValue("@ActionName", actionName);
                    parameters.AddWithValue("@ExceptionType", exceptionType);

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

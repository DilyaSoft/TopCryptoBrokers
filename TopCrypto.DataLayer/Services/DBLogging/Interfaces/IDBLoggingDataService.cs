using System.Threading.Tasks;

namespace TopCrypto.DataLayer.Services.DBLogging.Interfaces
{
    public interface IDBLoggingDataService
    {
        Task WriteLog(string stackTrace, string message, string actionName, string exceptionType);
    }
}
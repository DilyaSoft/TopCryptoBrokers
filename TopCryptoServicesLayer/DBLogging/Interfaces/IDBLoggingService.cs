using System;
using System.Threading.Tasks;

namespace TopCrypto.ServicesLayer.DBLogging.Interfaces
{
    public interface IDBLoggingService
    {
        Task WriteLog(Exception e, string actionName);
    }
}
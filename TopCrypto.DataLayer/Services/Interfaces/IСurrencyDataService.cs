using System.Threading.Tasks;

namespace TopCrypto.DataLayer.Services.Interfaces
{
    public interface IСurrencyDataService
    {
        Task<string[]> GetDataBaseIds();
        Task SaveIdsToTable(string[] listOfnames);
    }
}
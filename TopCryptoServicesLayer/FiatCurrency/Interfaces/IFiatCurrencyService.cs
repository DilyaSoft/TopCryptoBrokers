using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.FiatCurrency.Models;
using TopCrypto.ServicesLayer.Interfaces;

namespace TopCrypto.ServicesLayer.FiatCurrency.Interfaces
{
    public interface IFiatCurrencyService: IFilterService<FiatCurrencyDTO>
    {
        Task<string[]> GetApiIds();
        Task<string[]> GetDataBaseIds();
        Task SaveIdsToTable(string[] ids);
    }
}
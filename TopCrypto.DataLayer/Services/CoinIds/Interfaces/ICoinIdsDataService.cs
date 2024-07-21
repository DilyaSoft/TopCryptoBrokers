using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinIds.Models;

namespace TopCrypto.DataLayer.Services.CoinIds.Interfaces
{
    public interface ICoinIdsDataService
    {
        Task ClearAndInsertNewCache(IList<CoinIdsDTO> listDTOs);
        Task<IList<CoinIdsDTO>> GetAllCoinsIDFromDB();
    }
}
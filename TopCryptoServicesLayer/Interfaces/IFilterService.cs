using System.Collections.Generic;
using System.Threading.Tasks;

namespace TopCrypto.ServicesLayer.Interfaces
{
    /*CoinInfoDTO*/
    public interface IFilterService<T>
    {
        Task<IList<T>> GetFilteredData();
    }
}
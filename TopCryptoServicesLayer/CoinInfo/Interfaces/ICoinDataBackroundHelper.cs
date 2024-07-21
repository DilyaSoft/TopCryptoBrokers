using System.Threading.Tasks;

namespace TopCrypto.ServicesLayer.CoinInfo.Interfaces
{
    public interface ICoinInfoBackroundHelper
    {
        Task Execute(bool saveResult = true);
    }
}
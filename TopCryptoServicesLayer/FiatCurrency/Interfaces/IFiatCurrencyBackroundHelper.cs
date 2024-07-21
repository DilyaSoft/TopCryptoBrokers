using System.Threading.Tasks;

namespace TopCrypto.ServicesLayer.FiatCurrency.Interfaces
{
    public interface IFiatCurrencyBackroundHelper
    {
        Task Execute();
    }
}
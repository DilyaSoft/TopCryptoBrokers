namespace TopCrypto.ServicesLayer.WebSockets.Interfaces
{
    public interface ISocketDataWrapperHelpers
    {
        string WrapForFiatCurency(string jsonData);
        string WrapForCoinData(string jsonData, int countOfPrices, string grapthNodes);
    }
}
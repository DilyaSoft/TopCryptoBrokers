using System.Threading.Tasks;
using TopCrypto.ServicesLayer.WebSockets.Models;

namespace TopCrypto.ServicesLayer.WebSockets.Interfaces
{
    public interface IMarketDataSocketManagerHelper
    {
        Task OnMessageReceive(string IdInList
            , SocketManagerRecevidMessageDTO messageDTO
            , SocketDTO socketDTO
            , ISocketManager socketManager);
        SocketManagerRecevidMessageDTO TryParseReponseString(string wsMessage);
    }
}
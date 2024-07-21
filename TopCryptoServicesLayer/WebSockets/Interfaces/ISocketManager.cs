using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.WebSockets.Models;

namespace TopCrypto.ServicesLayer.WebSockets.Interfaces
{
    public interface ISocketManager
    {
        string AddSocket(SocketDTO socket);
        ReadOnlyDictionary<string, SocketDTO> GetReadonlySocketsDTO();
        SocketDTO GetSocketDTOById(string id);
        Task RemoveSocket(string id);
        Task SendMessageToAllAsync(string message);
        Task SendMessageAsync(string id, WebSocket socket, string message);
        Task<SocketManagerRecevidMessageDTO> Receive(string id);
    }
}
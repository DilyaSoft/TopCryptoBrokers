using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using TopCrypto.ServicesLayer.WebSockets.Interfaces;
using TopCrypto.ServicesLayer.WebSockets.Models;

namespace TopCrypto.ServicesLayer.WebSockets
{
    public class MarketDataSocketManager : ISocketManager
    {        
        private IMarketDataSocketManagerHelper _marketDataSocketManagerHelper;
        public MarketDataSocketManager(IMarketDataSocketManagerHelper marketDataSocketManagerHelper)
        {
            this._marketDataSocketManagerHelper = marketDataSocketManagerHelper;
        }

        private static ConcurrentDictionary<string, SocketDTO> _socketDTOs
            = new ConcurrentDictionary<string, SocketDTO>();

        public ReadOnlyDictionary<string, SocketDTO> GetReadonlySocketsDTO()
        {
            return new ReadOnlyDictionary<string, SocketDTO>(_socketDTOs);
        }

        public SocketDTO GetSocketDTOById(string id)
        {
            return _socketDTOs.TryGetValue(id, out SocketDTO socketDTO) ? socketDTO : null;
        }

        public string AddSocket(SocketDTO socket)
        {
            var id = Guid.NewGuid().ToString();
            return _socketDTOs.TryAdd(id, socket) ? id : string.Empty;
        }

        public async Task RemoveSocket(string id)
        {
            _socketDTOs.TryRemove(id, out SocketDTO socket);

            if (socket == null) return;
            await socket.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure
                    , "Closed by the WebSocketManager"
                    , CancellationToken.None);
        }

        public async Task SendMessageToAllAsync(string message)
        {

            foreach (var pair in _socketDTOs)
            {
                var socket = pair.Value.Socket;

                await SendMessageAsync(pair.Key, socket, message);
            }
        }

        public async Task SendMessageAsync(string id, WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open) return;
            try
            {
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message), 0, message.Length),
                                        WebSocketMessageType.Text,
                                        true,
                                        CancellationToken.None);
            }
            catch (WebSocketException e)
            {
                if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                {
                    _socketDTOs.TryRemove(id, out SocketDTO socketDTO);
                }
            }
        }

        public async Task<SocketManagerRecevidMessageDTO> Receive(string id)
        {
            var socketDTO = GetSocketDTOById(id);
            if (socketDTO == null) return null;

            var socket = socketDTO.Socket;

            var buffer = new byte[1024];
            using (MemoryStream ms = new MemoryStream())
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    await ms.WriteAsync(buffer, 0, buffer.Length);
                    Array.Clear(buffer, 0, buffer.Length);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await RemoveSocket(id);
                        return null;
                    }
                    else if (result.EndOfMessage)
                    {
                        //visiblePriceCount
                        string wsMessage = Encoding.UTF8.GetString(ms.ToArray());
                        ms.SetLength(0);

                        var message = _marketDataSocketManagerHelper.TryParseReponseString(wsMessage);
                        if (message != null)
                        {
                            await _marketDataSocketManagerHelper.OnMessageReceive(id, message, socketDTO, this);
                        }
                    }
                }
            }
            return null;
        }   
    }
}
